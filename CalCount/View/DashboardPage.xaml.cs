using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Graphics;

namespace CalCount.View;

[QueryProperty(nameof(Username), "username")]
public partial class DashboardPage : ContentPage
{
    private string username = string.Empty;
    private DateTime _currentSelectedDate = DateTime.Now.Date;
    // Selected date is read from Settings (Preferences) now

    public DashboardPage()
    {
        InitializeComponent();

        // No programmatic button needed; top buttons are declared in XAML

        // Charts are updated in OnAppearing to reflect current data
        // No MessagingCenter available; charts refresh on OnAppearing
    }

// Stacked bar drawable: each bar (meal) shows stacked segments (foods) with separators and labels
class StackedBarDrawable : IDrawable
{
    readonly IList<IList<float>> _segments;
    readonly IList<IList<string>>? _segLabels;
    readonly IList<string>? _mealLabels;

    public StackedBarDrawable(IList<IList<float>> segments, IList<IList<string>>? segLabels = null, IList<string>? mealLabels = null)
    {
        _segments = segments;
        _segLabels = segLabels;
        _mealLabels = mealLabels;
    }

    public void Draw(ICanvas canvas, RectF r)
    {
        canvas.FillColor = Colors.Transparent;
        canvas.FillRectangle(r);
        if (_segments == null || _segments.Count == 0) return;


        // compute totals per bar and max across bars for consistent scaling
        var totals = _segments.Select(s => s?.Sum() ?? 0f).ToList();
        float maxTotal = totals.Count > 0 ? totals.Max() : 0f;

        float leftPad = 56f;
        float rightPad = 24f;
        float topPad = 12f;
        float bottomPad = 60f;

        // determine a "nice" maximum for the y-axis so labels look clean and the scale adjusts as totals grow
        float niceMax;
        if (maxTotal <= 0)
        {
            niceMax = 100f; // default scale
        }
        else
        {
            // choose a small step (power of ten) so rounding is not too coarse; step = 10^(exp-1), but at least 1
            double exp = Math.Floor(Math.Log10(Math.Max(1, maxTotal)));
            double stepPow = Math.Pow(10, Math.Max(0, exp - 1));
            float step = (float)Math.Max(1.0, stepPow);
            niceMax = (float)(Math.Ceiling(maxTotal / step) * step);
        }

        // baseline axis and horizontal gridlines
        canvas.StrokeColor = Colors.LightGray;
        canvas.StrokeSize = 1f;
        // baseline
        canvas.DrawLine(r.Left + leftPad + 4f, r.Bottom - bottomPad - 2f, r.Right - rightPad - 4f, r.Bottom - bottomPad - 2f);

        // draw top and mid gridlines and labels
        canvas.FontSize = 10f;
        canvas.FontColor = Colors.Gray;
        // compute usable height for drawing bars/gridlines
        float usableHeight = Math.Max(4f, r.Height - topPad - bottomPad - 8f);
        // top
        float yTop = r.Bottom - bottomPad - (niceMax > 0 ? (niceMax / niceMax) * usableHeight : 0f);
        canvas.DrawLine(r.Left + leftPad + 4f, yTop, r.Right - rightPad - 4f, yTop);
        canvas.DrawString(Math.Round(niceMax).ToString(CultureInfo.InvariantCulture), r.Left + 8f, yTop - 6f, HorizontalAlignment.Left);
        // mid
        float midVal = niceMax / 2f;
        float yMid = r.Bottom - bottomPad - (niceMax > 0 ? (midVal / niceMax) * usableHeight : 0f);
        canvas.DrawLine(r.Left + leftPad + 4f, yMid, r.Right - rightPad - 4f, yMid);
        canvas.DrawString(Math.Round(midVal).ToString(CultureInfo.InvariantCulture), r.Left + 8f, yMid - 6f, HorizontalAlignment.Left);
        // zero label at baseline
        canvas.DrawString("0", r.Left + 8f, r.Bottom - bottomPad - 6f, HorizontalAlignment.Left);

        float usableWidth = Math.Max(10f, r.Width - leftPad - rightPad);
        float gap = Math.Max(6f, usableWidth * 0.06f);
        int n = _segments.Count;
        float w = Math.Max(20f, (usableWidth - gap * (n + 1)) / n);

        var colors = new[] { Colors.SteelBlue, Colors.Coral, Colors.MediumSeaGreen, Colors.Goldenrod, Colors.MediumPurple, Colors.Tomato };

        for (int i = 0; i < n; i++)
        {
            float x = r.Left + leftPad + gap + i * (w + gap);
            var segs = _segments[i] ?? new List<float>();
            float cum = 0f;
            // draw segments stacked from bottom up
            for (int j = 0; j < segs.Count; j++)
            {
                float segVal = segs[j];
                // scale each segment against the niceMax so bars remain proportional across meals
                float h = niceMax > 0 ? (segVal / niceMax) * usableHeight : 0f;
                // rectangle coords
                float segTop = r.Bottom - bottomPad - cum - h;
                float height = h;
                canvas.FillColor = colors[j % colors.Length];
                canvas.FillRectangle(x, segTop, w, height);

                // black separator line at top of this segment
                canvas.StrokeColor = Colors.Black;
                canvas.StrokeSize = 1f;
                canvas.DrawLine(x, segTop, x + w, segTop);

                // label inside segment if tall enough
                if (_segLabels != null && j < _segLabels[i].Count && height > 18f)
                {
                    var label = _segLabels[i][j];
                    canvas.FontSize = 10f;
                    canvas.FontColor = Colors.White;
                    canvas.DrawString(label, x + w / 2f, segTop + height / 2f - 6f, HorizontalAlignment.Center);
                }

                cum += h;
            }

            // draw meal label under bar
            if (_mealLabels != null && i < _mealLabels.Count)
            {
                canvas.FontSize = 10f;
                canvas.FontColor = Colors.Gray;
                canvas.DrawString(_mealLabels[i], x + w / 2f, r.Bottom - bottomPad + 10f, HorizontalAlignment.Center);
            }
        }
    }
}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateCharts();
    }

    private void SaveChartState()
    {
        // Save chart state to preferences so it persists when settings are changed
        try
        {
            var app = Application.Current as App;
            DateTime selectedDate = DateTime.Now.Date;
            var sel = Preferences.Get("SelectedDate", string.Empty);
            if (!string.IsNullOrWhiteSpace(sel) && DateTime.TryParse(sel, out var parsed))
                selectedDate = parsed.Date;

            var entries = app?.CalorieEntries.Where(e => e.Date == selectedDate).ToList() ?? new List<Models.CalorieEntry>();

            // Store the selected date so we know which data to display
            Preferences.Set("DashboardSelectedDate", selectedDate.ToString("o"));

            // Store meal data
            var meals = new[] { "Breakfast", "Lunch", "Dinner", "Snack" };
            for (int i = 0; i < meals.Length; i++)
            {
                var items = entries.Where(e => e.Meal == meals[i]).ToList();
                var mealCalories = string.Join(",", items.Select(it => it.Calories.ToString(CultureInfo.InvariantCulture)));
                var mealNames = string.Join("|", items.Select(it => string.IsNullOrWhiteSpace(it.Name) ? "Item" : it.Name));
                Preferences.Set($"DashboardMeal{i}Calories", mealCalories);
                Preferences.Set($"DashboardMeal{i}Names", mealNames);
            }

            // Store history data (last 7 days)
            var historyDates = Enumerable.Range(0, 7).Select(i => selectedDate.AddDays(-6 + i)).ToList();
            var historyTotals = historyDates.Select(d => (float)(app?.CalorieEntries.Where(e => e.Date == d).Sum(en => en.Calories) ?? 0)).ToList();
            var historyLabels = historyDates.Select(d => d.ToString("ddd")).ToList();
            Preferences.Set("DashboardHistoryTotals", string.Join(",", historyTotals.Select(h => h.ToString(CultureInfo.InvariantCulture))));
            Preferences.Set("DashboardHistoryLabels", string.Join(",", historyLabels));

            // Store macro data
            var totalToday = entries.Sum(e => e.Calories);
            var macroTotals = new[] { totalToday * 0.4f, totalToday * 0.3f, totalToday * 0.3f };
            Preferences.Set("DashboardMacroTotals", string.Join(",", macroTotals.Select(m => m.ToString(CultureInfo.InvariantCulture))));
        }
        catch
        {
            // Silently fail if save fails - charts will be regenerated on next load
        }
    }

    private void UpdateCharts()
    {
        var mealView = this.FindByName("MealBarChart") as GraphicsView;
        var historyView = this.FindByName("HistoryBarChart") as GraphicsView;
        var macroView = this.FindByName("MacroPieChart") as GraphicsView;

        // Load entries from App and aggregate
        var app = Application.Current as App;
        // determine which date is selected in Settings (stored in Preferences). Default to today.
        DateTime selectedDate = DateTime.Now.Date;
        var sel = Preferences.Get("SelectedDate", string.Empty);
        if (!string.IsNullOrWhiteSpace(sel) && DateTime.TryParse(sel, out var parsed))
            selectedDate = parsed.Date;

        // Store the current selected date for use in navigation
        _currentSelectedDate = selectedDate;

        var entries = app?.CalorieEntries.Where(e => e.Date == selectedDate).ToList() ?? new List<Models.CalorieEntry>();

        // Debug: log the total entries and filtered entries
        System.Diagnostics.Debug.WriteLine($"[Dashboard] Total entries in app: {app?.CalorieEntries.Count ?? 0}");
        System.Diagnostics.Debug.WriteLine($"[Dashboard] Selected date: {selectedDate:yyyy-MM-dd}");
        System.Diagnostics.Debug.WriteLine($"[Dashboard] Filtered entries for date: {entries.Count}");
        foreach (var e in entries)
        {
            System.Diagnostics.Debug.WriteLine($"  - {e.Name}: {e.Calories} cal ({e.Meal}) on {e.Date:yyyy-MM-dd}");
        }

        // Calculate BMR from settings (BMR formula) and compute activity-adjusted recommended calories (TDEE)
        var bmrLabel = this.FindByName<Label>("BMRValue");
        var tdeeLabel = this.FindByName<Label>("RecommendedCaloriesValue");
        try
        {
            var weightStr = Preferences.Get("Weight", string.Empty);
            var heightStr = Preferences.Get("Height", string.Empty);
            var ageStr = Preferences.Get("Age", string.Empty);
            var isMale = Preferences.Get("GenderIsMale", false);

            if (!string.IsNullOrWhiteSpace(weightStr) && !string.IsNullOrWhiteSpace(heightStr) && int.TryParse(ageStr, out var ageVal))
            {
                // stored weight in lb, height in inches -> convert to kg/cm
                if (double.TryParse(weightStr, NumberStyles.Float, CultureInfo.InvariantCulture, out var weightLb) || double.TryParse(weightStr, out weightLb))
                {
                    if (double.TryParse(heightStr, NumberStyles.Float, CultureInfo.InvariantCulture, out var heightIn) || double.TryParse(heightStr, out heightIn))
                    {
                        double weightKg = weightLb * 0.45359237;
                        double heightCm = heightIn * 2.54;
                        double bmr = (10.0 * weightKg) + (6.25 * heightCm) - (5.0 * ageVal) + (isMale ? 5.0 : -161.0);
                        var rec = (int)Math.Round(bmr);
                        if (bmrLabel != null)
                            bmrLabel.Text = rec.ToString(CultureInfo.InvariantCulture);

                        // activity multiplier from saved activity level
                        var activity = Preferences.Get("ActivityLevel", string.Empty);
                        double mult = 1.0;
                        switch (activity)
                        {
                            case "Sedentary": mult = 1.2; break;
                            case "Lightly active (Walking)": mult = 1.375; break;
                            case "Moderately active (Daily Workout)": mult = 1.55; break;
                            case "Very active (Heavy Training)": mult = 1.725; break;
                            default: mult = 1.0; break;
                        }

                        // apply weekly weight-change adjustment: each lb/week ~ 500 kcal/day
                        var weeklyStr = Preferences.Get("WeeklyChange", string.Empty);
                        double weeklyVal = 0.0;
                        if (!string.IsNullOrWhiteSpace(weeklyStr))
                        {
                            if (!double.TryParse(weeklyStr, NumberStyles.Float, CultureInfo.InvariantCulture, out weeklyVal))
                                double.TryParse(weeklyStr, out weeklyVal);
                        }

                        if (tdeeLabel != null)
                        {
                            var tdeeDouble = (bmr * mult) + (weeklyVal * 500.0);
                            var tdee = (int)Math.Round(tdeeDouble);
                            tdeeLabel.Text = tdee.ToString(CultureInfo.InvariantCulture);
                        }
                    }
                    else
                    {
                        if (bmrLabel != null) bmrLabel.Text = "--";
                        if (tdeeLabel != null) tdeeLabel.Text = "--";
                    }
                }
                else
                {
                    if (bmrLabel != null) bmrLabel.Text = "--";
                    if (tdeeLabel != null) tdeeLabel.Text = "--";
                }
            }
            else
            {
                if (bmrLabel != null) bmrLabel.Text = "--";
                if (tdeeLabel != null) tdeeLabel.Text = "--";
            }
        }
        catch
        {
            if (this.FindByName<Label>("BMRValue") is Label lb) lb.Text = "--";
            if (this.FindByName<Label>("RecommendedCaloriesValue") is Label lt) lt.Text = "--";
        }

        // Aggregate by meal for the meal chart, but keep per-food segments so we can draw stacked bars
        var meals = new[] { "Breakfast", "Lunch", "Dinner", "Snack" };
        var perMealSegments = new List<List<float>>();
        var perMealLabels = new List<List<string>>();
        foreach (var m in meals)
        {
            var items = entries.Where(e => e.Meal == m).ToList();
            var segs = new List<float>();
            var labs = new List<string>();
            foreach (var it in items)
            {
                segs.Add(it.Calories);
                labs.Add(string.IsNullOrWhiteSpace(it.Name) ? "Item" : it.Name);
            }
            perMealSegments.Add(segs);
            perMealLabels.Add(labs);
        }

        // Daily history - last 7 days total calories (chronological)
        var historyDates = Enumerable.Range(0, 7).Select(i => selectedDate.AddDays(-6 + i)).ToList();
        var historyTotals = historyDates.Select(d => (float)(app?.CalorieEntries.Where(e => e.Date == d).Sum(en => en.Calories) ?? 0)).ToList();
        var historyLabels = historyDates.Select(d => d.ToString("ddd")).ToList();

        // Macro breakdown - recommended ratios: 40% carbs, 30% protein, 30% fat
        var totalToday = entries.Sum(e => e.Calories);
        var macroTotals = new List<float> { (float)(totalToday * 0.4), (float)(totalToday * 0.3), (float)(totalToday * 0.3) };
        var macroLabels = new List<string> { "Carbs", "Protein", "Fat" };

        if (mealView != null)
        {
            // Use a stacked bar drawable so each food appears as a stacked segment within the meal bar
            mealView.Drawable = new StackedBarDrawable(perMealSegments.Select(s => (IList<float>)s).ToList(), perMealLabels.Select(s => (IList<string>)s).ToList(), meals.ToList());
            mealView.Invalidate();
        }

        if (historyView != null)
        {
            // Title is shown above the view in XAML; avoid drawing it inside the chart to prevent duplication
            historyView.Drawable = new SmallBarDrawable(historyTotals, historyLabels);
            historyView.Invalidate();
        }

        if (macroView != null)
        {
            macroView.Drawable = new SmallPieDrawable(macroTotals, macroLabels);
            macroView.Invalidate();
        }

        // Save the chart state so it persists across navigation and settings changes
        SaveChartState();
    }

    // Day selection and clearing moved to Settings page

    public string Username
    {
        get => username;
        set
        {
            username = value;
            if (!string.IsNullOrEmpty(username) && this.FindByName("WelcomeLabel") is Label lbl)
            {
                lbl.Text = $"Welcome, {username}!";
            }
        }
    }

    private async void OnRecipesClicked(object? sender, EventArgs e)
    {
        // Use Shell navigation so routes are resolved consistently
        await Shell.Current.GoToAsync("recipes");
    }

    private async void OnAddFoodClicked(object? sender, EventArgs e)
    {
        var dateParam = Uri.EscapeDataString(_currentSelectedDate.ToString("o"));
        await Shell.Current.GoToAsync($"addfood?date={dateParam}");
    }

    private async void OnSettingsClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("settings");
    }
}

// Small, concise bar drawable
class SmallBarDrawable : IDrawable
{
    readonly IList<float> _values;
    readonly IList<string>? _labels;
    readonly string? _title;
    public SmallBarDrawable(IList<float> values, IList<string>? labels = null, string? title = null)
    {
        _values = values;
        _labels = labels;
        _title = title;
    }
    public void Draw(ICanvas canvas, RectF r)
    {
        canvas.FillColor = Colors.Transparent;
        canvas.FillRectangle(r);
        if (_values == null || _values.Count == 0) return;
        // reserve space for title, y-axis labels and bar labels so text doesn't get clipped
        float leftPad = 56f; // more space for Y-axis labels and left margin
        float rightPad = 24f;
        float topPad = string.IsNullOrEmpty(_title) ? 12f : 36f;
        float bottomPad = (_labels != null && _labels.Count > 0) ? 60f : 18f;

        // draw baseline axis so empty charts still show structure
        canvas.StrokeColor = Colors.LightGray;
        canvas.StrokeSize = 1f;
        canvas.DrawLine(r.Left + leftPad + 4f, r.Bottom - bottomPad - 2f, r.Right - rightPad - 4f, r.Bottom - bottomPad - 2f);

        float max = _values.Max();
        float usableWidth = Math.Max(10f, r.Width - leftPad - rightPad);
        float gap = Math.Max(6f, usableWidth * 0.06f);
        float w = Math.Max(10f, (usableWidth - gap * (_values.Count + 1)) / _values.Count);
        // colors correspond to macroLabels order: Carbs, Protein, Fat
        var cols = new[] { Colors.Goldenrod, Colors.MediumSeaGreen, Colors.Tomato };
        for (int i = 0; i < _values.Count; i++)
        {
            float usableHeight = Math.Max(4f, r.Height - topPad - bottomPad - 8f);
            float h = max > 0 ? (_values[i] / max) * usableHeight : 0f;
            float x = r.Left + leftPad + gap + i * (w + gap);
            canvas.FillColor = cols[i % cols.Length];
            // draw the filled bar
            canvas.FillRectangle(x, r.Bottom - bottomPad - h, w, h);
            // draw a clear black line at the top of the bar to separate from next entry
            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 1f;
            canvas.DrawLine(x, r.Bottom - bottomPad - h, x + w, r.Bottom - bottomPad - h);
        }

        // Draw numeric values above each bar and optionally draw label inside the bar
        for (int i = 0; i < _values.Count; i++)
        {
            float usableHeight = Math.Max(4f, r.Height - topPad - bottomPad - 8f);
            float h = max > 0 ? (_values[i] / max) * usableHeight : 0f;
            float x = r.Left + leftPad + gap + i * (w + gap);
            var txtVal = Math.Round(_values[i]).ToString();
            float txtY = r.Bottom - bottomPad - h - 14f;
            // ensure text doesn't overlap title area
            txtY = Math.Max(txtY, r.Top + 8f);
            canvas.FontSize = 10f;
            // if the value label would overlap the bar, draw it above in black; otherwise black by default
            canvas.FontColor = Colors.Black;
            canvas.DrawString(txtVal, x + w / 2f, txtY, HorizontalAlignment.Center);

            // draw the bar label (food/meal name) inside the bar when there's space; otherwise it'll remain under the bar later
            if (_labels != null && i < _labels.Count)
            {
                var label = _labels[i];
                // compute a vertical center inside the bar
                float labelY = r.Bottom - bottomPad - h + (h / 2f);
                // only draw inside if bar is tall enough
                if (h > 18f)
                {
                    canvas.FontSize = 10f;
                    canvas.FontColor = Colors.White;
                    // ensure text fits horizontally by trimming if necessary
                    canvas.DrawString(label, x + w / 2f, labelY - 6f, HorizontalAlignment.Center);
                }
            }
        }

        // Draw labels under each bar if provided
        if (_labels != null)
        {
            canvas.FontColor = Colors.Gray;
            canvas.FontSize = 10f;
            for (int i = 0; i < Math.Min(_labels.Count, _values.Count); i++)
            {
                float x = r.Left + leftPad + gap + i * (w + gap);
                var txt = _labels[i];
                // center text within the reserved bottom padding
                canvas.DrawString(txt, x + w / 2f, r.Bottom - bottomPad + 10f, HorizontalAlignment.Center);
            }
        }

        // Draw simple Y-axis labels (max and mid) so numbers are easier to read
        if (max > 0)
        {
            canvas.FontSize = 10f;
            canvas.FontColor = Colors.Gray;
            // top (max)
            canvas.DrawString(Math.Round(max).ToString(), r.Left + 8f, r.Top + 4f, HorizontalAlignment.Left);
            // mid (~half)
            canvas.DrawString(Math.Round(max / 2f).ToString(), r.Left + 8f, r.Center.Y - 6f, HorizontalAlignment.Left);
            // zero at baseline
            canvas.DrawString("0", r.Left + 8f, r.Bottom - bottomPad - 6f, HorizontalAlignment.Left);
        }

        // Draw title at top if provided, centered over the usable chart area
        if (!string.IsNullOrEmpty(_title))
        {
            canvas.FontColor = Colors.Gray;
            canvas.FontSize = 12f;
            // center title over the usable area (account for left/right padding)
            float usableWidthForTitle = Math.Max(10f, r.Width - leftPad - rightPad);
            float titleX = r.Left + leftPad + (usableWidthForTitle / 2f);
            canvas.DrawString(_title, titleX, r.Top + (topPad / 2f) - 4f, HorizontalAlignment.Center);
        }
    }
}

// Small, concise pie drawable
class SmallPieDrawable : IDrawable
{
    readonly IList<float> _values;
    readonly IList<string>? _labels;
    public SmallPieDrawable(IList<float> values, IList<string>? labels = null)
    {
        _values = values;
        _labels = labels;
    }
    public void Draw(ICanvas canvas, RectF r)
    {
        canvas.FillColor = Colors.Transparent;
        canvas.FillRectangle(r);
        if (_values == null || _values.Count == 0) return;
        float total = _values.Sum(); if (total <= 0) return;
        float cx = r.Center.X, cy = r.Center.Y, rad = Math.Min(r.Width, r.Height) * 0.4f;
        var cols = new[] { Colors.Goldenrod, Colors.MediumSeaGreen, Colors.Tomato };
        float start = -90f;
        // draw slices with a small gap between them so colors are distinct
        float gapAngle = 1.5f; // degrees of gap between slices
        for (int i = 0; i < _values.Count; i++)
        {
            float sweep = (_values[i] / total) * 360f;
            if (sweep <= 0) continue;

            // reduce sweep slightly to create a visible gap, but avoid negative sweep
            float drawSweep = Math.Max(0f, sweep - gapAngle);
            float drawStart = start + (sweep - drawSweep) / 2f;

            canvas.FillColor = cols[i % cols.Length];
            canvas.FillArc(cx - rad, cy - rad, rad * 2f, rad * 2f, drawStart, drawSweep, true);

            start += sweep;
        }

        // draw an outline circle to improve contrast
        canvas.StrokeColor = Colors.Gray;
        canvas.StrokeSize = 1f;
        canvas.DrawArc(cx - rad, cy - rad, rad * 2f, rad * 2f, -90f, 360f, false, false);

        // Draw labels to the right of pie (legend)
        if (_labels != null)
        {
            float lx = r.Right - 140f;
            float ly = r.Top + 8f;
            canvas.FontSize = 12f;
            for (int i = 0; i < Math.Min(_labels.Count, _values.Count); i++)
            {
                canvas.FillColor = cols[i % cols.Length];
                canvas.FillRectangle(lx, ly + i * 22, 12, 12);
                canvas.FontColor = Colors.Gray;
                // include numeric calorie values and percentage
                var percent = Math.Round((_values[i] / total) * 100);
                var val = Math.Round(_values[i]);
                canvas.DrawString(_labels[i] + " - " + val + " cal (" + percent + "%)", lx + 18, ly + i * 22, HorizontalAlignment.Left);
            }
        }
    }
}


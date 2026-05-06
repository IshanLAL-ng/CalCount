using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace CalCount.View;

[QueryProperty(nameof(Username), "username")]
public partial class DashboardPage : ContentPage
{
    private string username = string.Empty;

    public DashboardPage()
    {
        InitializeComponent();

        // No programmatic button needed; top buttons are declared in XAML

        // Charts are updated in OnAppearing to reflect current data
        // No MessagingCenter available; charts refresh on OnAppearing
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateCharts();
    }

    private void UpdateCharts()
    {
        var mealView = this.FindByName("MealBarChart") as GraphicsView;
        var historyView = this.FindByName("HistoryBarChart") as GraphicsView;
        var macroView = this.FindByName("MacroPieChart") as GraphicsView;

        // Load entries from App and aggregate
        var app = Application.Current as App;
        var today = DateTime.Now.Date;
        var entries = app?.CalorieEntries.Where(e => e.Date == today).ToList() ?? new List<Models.CalorieEntry>();

        // Aggregate by meal for the meal chart (include Snack/Other so entries aren't missed)
        var meals = new[] { "Breakfast", "Lunch", "Dinner", "Snack", "Other" };
        var mealTotals = meals.Select(m => (float)entries.Where(e => e.Meal == m).Sum(e => e.Calories)).ToList();
        var mealLabels = meals.ToList();

        // Daily history - last 7 days total calories (chronological)
        var historyDates = Enumerable.Range(0, 7).Select(i => today.AddDays(-6 + i)).ToList();
        var historyTotals = historyDates.Select(d => (float)(app?.CalorieEntries.Where(e => e.Date == d).Sum(en => en.Calories) ?? 0)).ToList();
        var historyLabels = historyDates.Select(d => d.ToString("ddd")).ToList();

        // Macro breakdown - placeholder values calculated from today's totals (split by fixed ratios)
        var totalToday = entries.Sum(e => e.Calories);
        var macroTotals = new List<float> { (float)(totalToday * 0.4), (float)(totalToday * 0.35), (float)(totalToday * 0.25) };
        var macroLabels = new List<string> { "Protein", "Carbs", "Fat" };

        if (mealView != null)
        {
            mealView.Drawable = new SmallBarDrawable(mealTotals, mealLabels, "Calories by Meal");
            mealView.Invalidate();
        }

        if (historyView != null)
        {
            historyView.Drawable = new SmallBarDrawable(historyTotals, historyLabels, "Daily Calories");
            historyView.Invalidate();
        }

        if (macroView != null)
        {
            macroView.Drawable = new SmallPieDrawable(macroTotals, macroLabels);
            macroView.Invalidate();
        }
    }

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
        await Shell.Current.GoToAsync("addfood");
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
        // reserve space for title and labels so text doesn't get clipped
        float topPad = string.IsNullOrEmpty(_title) ? 6f : 22f;
        float bottomPad = (_labels != null && _labels.Count > 0) ? 20f : 6f;

        // draw baseline axis so empty charts still show structure
        canvas.StrokeColor = Colors.LightGray;
        canvas.StrokeSize = 1f;
        canvas.DrawLine(r.Left + 2f, r.Bottom - bottomPad - 2f, r.Right - 2f, r.Bottom - bottomPad - 2f);
        float max = _values.Max();
        float gap = 6f;
        float w = Math.Max(6f, (r.Width - gap * (_values.Count + 1)) / _values.Count);
        var cols = new[] { Colors.SteelBlue, Colors.Coral, Colors.MediumSeaGreen };
        for (int i = 0; i < _values.Count; i++)
        {
            float usableHeight = Math.Max(4f, r.Height - topPad - bottomPad - 8f);
            float h = max > 0 ? (_values[i] / max) * usableHeight : 0f;
            float x = r.Left + gap + i * (w + gap);
            canvas.FillColor = cols[i % cols.Length];
            canvas.FillRectangle(x, r.Bottom - bottomPad - h, w, h);
        }

        // Draw labels under each bar if provided
        if (_labels != null)
        {
            canvas.FontColor = Colors.Gray;
            canvas.FontSize = 10f;
            for (int i = 0; i < Math.Min(_labels.Count, _values.Count); i++)
            {
                float x = r.Left + gap + i * (w + gap);
                var txt = _labels[i];
                // center text within the reserved bottom padding
                canvas.DrawString(txt, x + w / 2f, r.Bottom - bottomPad + 4f, HorizontalAlignment.Center);
            }
        }

        // Draw title at top if provided
        if (!string.IsNullOrEmpty(_title))
        {
            canvas.FontColor = Colors.Gray;
            canvas.FontSize = 12f;
            // draw title in the reserved top padding
            canvas.DrawString(_title, r.Center.X, r.Top + 6f, HorizontalAlignment.Center);
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
        float cx = r.Center.X, cy = r.Center.Y, rad = Math.Min(r.Width, r.Height) * 0.35f;
        var cols = new[] { Colors.SteelBlue, Colors.Coral, Colors.MediumSeaGreen };
        float start = -90f;
        for (int i = 0; i < _values.Count; i++)
        {
            float sweep = (_values[i] / total) * 360f;
            canvas.FillColor = cols[i % cols.Length];
            canvas.FillArc(cx - rad, cy - rad, rad * 2f, rad * 2f, start, sweep, true);
            start += sweep;
        }

        // Draw labels to the right of pie
        if (_labels != null)
        {
            float lx = r.Right - 80f;
            float ly = r.Top + 8f;
            canvas.FontSize = 12f;
            for (int i = 0; i < Math.Min(_labels.Count, _values.Count); i++)
            {
                canvas.FillColor = cols[i % cols.Length];
                canvas.FillRectangle(lx, ly + i * 18, 12, 12);
                canvas.FontColor = Colors.Gray;
                canvas.DrawString(_labels[i] + " (" + Math.Round((_values[i] / total) * 100) + "%)", lx + 18, ly + i * 18, HorizontalAlignment.Left);
            }
        }
    }
}


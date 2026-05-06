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

        // Daily history - last 7 days total calories
        var historyTotals = Enumerable.Range(0, 7).Select(i =>
        {
            var d = today.AddDays(-i);
            return (float)(app?.CalorieEntries.Where(e => e.Date == d).Sum(en => en.Calories) ?? 0);
        }).Reverse().ToList();

        // Macro breakdown - placeholder values calculated from today's totals (split by fixed ratios)
        var totalToday = entries.Sum(e => e.Calories);
        var macroTotals = new List<float> { (float)(totalToday * 0.4), (float)(totalToday * 0.35), (float)(totalToday * 0.25) };

        if (mealView != null)
        {
            mealView.Drawable = new SmallBarDrawable(mealTotals);
            mealView.Invalidate();
        }

        if (historyView != null)
        {
            historyView.Drawable = new SmallBarDrawable(historyTotals);
            historyView.Invalidate();
        }

        if (macroView != null)
        {
            macroView.Drawable = new SmallPieDrawable(macroTotals);
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
    public SmallBarDrawable(IList<float> values) => _values = values;
    public void Draw(ICanvas canvas, RectF r)
    {
        canvas.FillColor = Colors.Transparent;
        canvas.FillRectangle(r);
        if (_values == null || _values.Count == 0) return;
        // draw baseline axis so empty charts still show structure
        canvas.StrokeColor = Colors.LightGray;
        canvas.StrokeSize = 1f;
        canvas.DrawLine(r.Left + 2f, r.Bottom - 2f, r.Right - 2f, r.Bottom - 2f);
        float max = _values.Max();
        float gap = 6f;
        float w = Math.Max(6f, (r.Width - gap * (_values.Count + 1)) / _values.Count);
        var cols = new[] { Colors.SteelBlue, Colors.Coral, Colors.MediumSeaGreen };
        for (int i = 0; i < _values.Count; i++)
        {
            float h = max > 0 ? (_values[i] / max) * (r.Height - 8f) : 0f;
            float x = r.Left + gap + i * (w + gap);
            canvas.FillColor = cols[i % cols.Length];
            canvas.FillRectangle(x, r.Bottom - h, w, h);
        }
    }
}

// Small, concise pie drawable
class SmallPieDrawable : IDrawable
{
    readonly IList<float> _values;
    public SmallPieDrawable(IList<float> values) => _values = values;
    public void Draw(ICanvas canvas, RectF r)
    {
        canvas.FillColor = Colors.Transparent;
        canvas.FillRectangle(r);
        if (_values == null || _values.Count == 0) return;
        float total = _values.Sum(); if (total <= 0) return;
        float cx = r.Center.X, cy = r.Center.Y, rad = Math.Min(r.Width, r.Height) * 0.4f;
        var cols = new[] { Colors.SteelBlue, Colors.Coral, Colors.MediumSeaGreen };
        float start = -90f;
        for (int i = 0; i < _values.Count; i++)
        {
            float sweep = (_values[i] / total) * 360f;
            canvas.FillColor = cols[i % cols.Length];
            canvas.FillArc(cx - rad, cy - rad, rad * 2f, rad * 2f, start, sweep, true);
            start += sweep;
        }
    }
}


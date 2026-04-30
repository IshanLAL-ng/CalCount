using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace CalCount.View;

public partial class DashboardPage : ContentPage
{
    public DashboardPage(string username)
    {
        InitializeComponent();
        WelcomeLabel.Text = $"Welcome, {username}!";
        // Add a Recipes button programmatically in case editing the XAML is problematic
        if (this.Content is ScrollView sv && sv.Content is VerticalStackLayout vsl)
        {
            var recipesBtn = new Button
            {
                Text = "Recipes",
                HorizontalOptions = LayoutOptions.Center
            };

            recipesBtn.Clicked += OnRecipesClicked;
            vsl.Children.Add(recipesBtn);
        }

        // small sample data for the charts
        var mealData = new List<float> { 600f, 450f, 300f };
        var historyData = new List<float> { 1800f, 2000f, 1750f, 1900f, 2100f, 1600f, 1850f };
        var macroData = new List<float> { 40f, 30f, 30f };

        var mealView = this.FindByName("MealBarChart") as GraphicsView;
        var historyView = this.FindByName("HistoryBarChart") as GraphicsView;
        var macroView = this.FindByName("MacroPieChart") as GraphicsView;

        if (mealView != null)
        {
            mealView.Drawable = new SmallBarDrawable(mealData);
            mealView.Invalidate();
        }

        if (historyView != null)
        {
            historyView.Drawable = new SmallBarDrawable(historyData);
            historyView.Invalidate();
        }

        if (macroView != null)
        {
            macroView.Drawable = new SmallPieDrawable(macroData);
            macroView.Invalidate();
        }
    }

    private async void OnRecipesClicked(object? sender, EventArgs e)
    {
        // Navigate to the recipes page
        await Navigation.PushAsync(new RecipesPage());
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


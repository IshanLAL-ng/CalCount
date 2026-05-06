using System;
using System.Globalization;
using Microsoft.Maui.Storage;

namespace CalCount.View;

public partial class SettingsPage : ContentPage
{
    const string PrefHeight = "Height";
    const string PrefWeight = "Weight";
    const string PrefAge = "Age";
    const string PrefGenderIsMale = "GenderIsMale";
    const string PrefWeeklyChange = "WeeklyChange";

    public SettingsPage()
    {
        InitializeComponent();

        LoadSettings();
    }

    void LoadSettings()
    {
        try
        {
            var height = this.FindByName<Entry>("HeightEntry");
            var weight = this.FindByName<Entry>("WeightEntry");
            var age = this.FindByName<Entry>("AgeEntry");
            var weekly = this.FindByName<Entry>("WeeklyChangeEntry");
            var genderSwitch = this.FindByName<Switch>("GenderSwitch");
            var genderLabel = this.FindByName<Label>("GenderLabel");

            if (height != null)
                height.Text = Preferences.Get(PrefHeight, string.Empty);
            if (weight != null)
                weight.Text = Preferences.Get(PrefWeight, string.Empty);
            if (age != null)
                age.Text = Preferences.Get(PrefAge, string.Empty);
            if (weekly != null)
                weekly.Text = Preferences.Get(PrefWeeklyChange, string.Empty);
            if (genderSwitch != null)
            {
                genderSwitch.IsToggled = Preferences.Get(PrefGenderIsMale, false);
                if (genderLabel != null)
                    genderLabel.Text = genderSwitch.IsToggled ? "Male" : "Female";
            }
        }
        catch
        {
            // ignore preferences errors
        }
    }

    void OnGenderToggled(object sender, ToggledEventArgs e)
    {
        var label = this.FindByName<Label>("GenderLabel");
        if (label != null)
            label.Text = e.Value ? "Male" : "Female";
    }

    void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            var height = this.FindByName<Entry>("HeightEntry");
            var weight = this.FindByName<Entry>("WeightEntry");
            var age = this.FindByName<Entry>("AgeEntry");
            var weekly = this.FindByName<Entry>("WeeklyChangeEntry");
            var genderSwitch = this.FindByName<Switch>("GenderSwitch");

            // validate weekly change in range -5..5
            if (weekly != null && !string.IsNullOrWhiteSpace(weekly.Text))
            {
                if (!double.TryParse(weekly.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out var val) && !double.TryParse(weekly.Text, out val))
                {
                    DisplayAlert("Validation", "Weekly change must be a number between -5 and 5.", "OK");
                    return;
                }
                if (val < -5 || val > 5)
                {
                    DisplayAlert("Validation", "Weekly change must be between -5 and 5 pounds per week.", "OK");
                    return;
                }
            }

            Preferences.Set(PrefHeight, height?.Text ?? string.Empty);
            Preferences.Set(PrefWeight, weight?.Text ?? string.Empty);
            Preferences.Set(PrefAge, age?.Text ?? string.Empty);
            Preferences.Set(PrefWeeklyChange, weekly?.Text ?? string.Empty);
            Preferences.Set(PrefGenderIsMale, genderSwitch?.IsToggled ?? false);

            DisplayAlert("Settings", "Saved.", "OK");
        }
        catch
        {
            DisplayAlert("Settings", "Unable to save settings.", "OK");
        }
    }
}

using System;
using System.Globalization;
using System.Linq;
using Microsoft.Maui.Storage;

namespace CalCount.View;

public partial class SettingsPage : ContentPage
{
    List<DateTime> _pickerDates = new List<DateTime>();
    const string PrefHeight = "Height";
    const string PrefWeight = "Weight";
    const string PrefAge = "Age";
    const string PrefGenderIsMale = "GenderIsMale";
    const string PrefWeeklyChange = "WeeklyChange";
    const string PrefSelectedDate = "SelectedDate";
    const string PrefActivityLevel = "ActivityLevel";

    public SettingsPage()
    {
        InitializeComponent();

        LoadSettings();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // ensure the day picker is populated when the page appears
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

            // Populate DayPicker with last 7 days and select saved date
            var picker = this.FindByName<Picker>("DayPicker");
            var activity = this.FindByName<Picker>("ActivityPicker");
            // populate activity picker selection if present
            if (activity != null)
            {
                var savedAct = Preferences.Get(PrefActivityLevel, string.Empty);
                if (!string.IsNullOrEmpty(savedAct))
                {
                    var idx = activity.Items.IndexOf(savedAct);
                    if (idx >= 0)
                        activity.SelectedIndex = idx;
                }
            }
            _pickerDates.Clear();
            if (picker != null)
            {
                picker.Items.Clear();
                var today = DateTime.Now.Date;
                var dates = Enumerable.Range(0, 7).Select(i => today.AddDays(-6 + i)).ToList();
                foreach (var d in dates)
                {
                    picker.Items.Add(d.DayOfWeek.ToString());
                    _pickerDates.Add(d);
                }

                // select saved date if present
                var sel = Preferences.Get(PrefSelectedDate, string.Empty);
                if (!string.IsNullOrWhiteSpace(sel) && DateTime.TryParse(sel, out var parsed))
                {
                    var idx = _pickerDates.FindIndex(x => x == parsed.Date);
                    if (idx >= 0)
                        picker.SelectedIndex = idx;
                }
                else
                {
                    picker.SelectedIndex = _pickerDates.Count - 1; // today
                }
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

    async void OnSaveClicked(object sender, EventArgs e)
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

            // save activity selection
            var activity = this.FindByName<Picker>("ActivityPicker");
            if (activity != null && activity.SelectedIndex >= 0 && activity.SelectedIndex < activity.Items.Count)
            {
                Preferences.Set(PrefActivityLevel, activity.Items[activity.SelectedIndex]);
            }

            // save selected day preference
            var picker = this.FindByName<Picker>("DayPicker");
            if (picker != null && picker.SelectedIndex >= 0 && picker.SelectedIndex < _pickerDates.Count)
            {
                Preferences.Set(PrefSelectedDate, _pickerDates[picker.SelectedIndex].ToString("o"));
            }

            await DisplayAlert("Settings", "Saved.", "OK");

            // return to previous page (Dashboard) so it can refresh and show updated recommended calories
            // The Dashboard's OnAppearing will be called automatically and will regenerate charts
            if (Navigation.NavigationStack.Count > 0)
                await Navigation.PopAsync();
        }
        catch
        {
            await DisplayAlert("Settings", "Unable to save settings.", "OK");
        }
    }

    private async void OnClearDayClicked(object? sender, EventArgs e)
    {
        var picker = this.FindByName<Picker>("DayPicker");
        if (picker == null || picker.SelectedIndex < 0 || picker.SelectedIndex >= _pickerDates.Count)
        {
            await DisplayAlert("Error", "Please select a day to clear.", "OK");
            return;
        }

        var selectedDate = _pickerDates[picker.SelectedIndex];
        bool ok = await DisplayAlert("Confirm", $"Remove all entries for {selectedDate:d}?", "Yes", "No");
        if (!ok) return;

        var app = Application.Current as App;
        app?.RemoveEntriesForDate(selectedDate);
        await DisplayAlert("Cleared", $"Entries for {selectedDate:d} removed.", "OK");
    }
}

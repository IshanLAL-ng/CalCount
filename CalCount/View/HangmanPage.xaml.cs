using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace CalCount.View;

public partial class HangmanPage : ContentPage
{
    readonly List<string> words = new() { "apple", "banana", "orange", "computer", "microsoft", "maui", "developer", "keyboard" };
    string currentWord = string.Empty;
    HashSet<char> guessed = new();
    int wrongGuesses = 0;
    const int maxWrong = 6;

    public HangmanPage()
    {
        InitializeComponent();
        StartNewWord();
    }

    void StartNewWord()
    {
        var rnd = new Random();
        currentWord = words[rnd.Next(words.Count)].ToUpperInvariant();
        guessed.Clear();
        wrongGuesses = 0;
        BuildLettersGrid();
        UpdateDisplay();
    }

    void BuildLettersGrid()
    {
        LettersGrid.Children.Clear();
        LettersGrid.RowDefinitions.Clear();
        LettersGrid.ColumnDefinitions.Clear();

        int cols = 7;
        for (int c = 0; c < cols; c++) LettersGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

        int rows = 4;
        for (int r = 0; r < rows; r++) LettersGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int index = 0;
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (index >= letters.Length) break;
                var ch = letters[index++];
                var btn = new Button { Text = ch.ToString(), FontSize = 14 };
                btn.Clicked += OnLetterClicked;
                LettersGrid.Add(btn, c, r);
            }
        }
    }

    void UpdateDisplay()
    {
        var display = new System.Text.StringBuilder();
        bool won = true;
        foreach (var ch in currentWord)
        {
            if (char.IsLetter(ch))
            {
                if (guessed.Contains(ch)) display.Append(ch + " "); else { display.Append("_ "); won = false; }
            }
            else display.Append(ch + " ");
        }

        WordLabel.Text = display.ToString();
        StatusLabel.Text = $"Wrong: {wrongGuesses} / {maxWrong}";

        if (won)
        {
            DisplayAlert("You win", $"The word was {currentWord}", "OK");
        }
        else if (wrongGuesses >= maxWrong)
        {
            DisplayAlert("You lose", $"The word was {currentWord}", "OK");
        }
    }

    void OnLetterClicked(object? sender, EventArgs e)
    {
        if (sender is Button b && !string.IsNullOrEmpty(b.Text))
        {
            b.IsEnabled = false;
            var ch = b.Text[0];
            if (currentWord.Contains(ch)) guessed.Add(ch); else wrongGuesses++;
            UpdateDisplay();
        }
    }

    void OnNewWordClicked(object sender, EventArgs e)
    {
        StartNewWord();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;

namespace CalCount.View;

public partial class HangmanPage : ContentPage
{
    // words not related to software/work
    readonly List<string> words = new()
    {
        "elephant",
        "pineapple",
        "mountain",
        "sunflower",
        "giraffe",
        "hippopotamus",
        "strawberry",
        "kangaroo",
        "waterfall",
        "butterfly"
    };
    string currentWord = string.Empty;
    HashSet<char> guessed = new();
    int wrongGuesses = 0;
    const int maxWrong = 6;

    public HangmanPage()
    {
        InitializeComponent();
        // wire Completed in code to avoid XAML event binding issues on some platforms
        GuessEntry.Completed += OnGuessCompleted;
        StartNewWord();
    }

    void StartNewWord()
    {
        var rnd = new Random();
        currentWord = words[rnd.Next(words.Count)].ToUpperInvariant();
        guessed.Clear();
        wrongGuesses = 0;
        GuessEntry.Text = string.Empty;
        GuessEntry.Focus();
        UpdateDisplay();
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
        GuessedLabel.Text = "Guessed: " + string.Join(", ", guessed.OrderBy(c => c));

        if (won)
        {
            DisplayAlert("You win", $"The word was {currentWord}", "OK");
        }
        else if (wrongGuesses >= maxWrong)
        {
            DisplayAlert("You lose", $"The word was {currentWord}", "OK");
        }
    }
    void OnNewWordClicked(object sender, EventArgs e)
    {
        StartNewWord();
    }

    void OnGuessClicked(object sender, EventArgs e)
    {
        ProcessGuess(GuessEntry?.Text ?? string.Empty);
    }

    void OnGuessCompleted(object? sender, EventArgs e)
    {
        ProcessGuess(GuessEntry?.Text ?? string.Empty);
    }

    void ProcessGuess(string raw)
    {
        var text = (raw ?? string.Empty).Trim().ToUpperInvariant();
        if (string.IsNullOrEmpty(text)) return;

        // Full-word guess
        if (text.Length > 1)
        {
            if (text == currentWord)
            {
                foreach (var c in currentWord) if (char.IsLetter(c)) guessed.Add(c);
            }
            else
            {
                // penalize one wrong guess for an incorrect full-word guess
                wrongGuesses++;
            }
            GuessEntry.Text = string.Empty;
            UpdateDisplay();
            return;
        }

        var ch = text[0];
        if (!char.IsLetter(ch))
        {
            DisplayAlert("Invalid", "Please type a letter (A-Z).", "OK");
            GuessEntry.Text = string.Empty;
            return;
        }

        if (guessed.Contains(ch))
        {
            // already guessed
            GuessEntry.Text = string.Empty;
            return;
        }

        guessed.Add(ch);
        if (!currentWord.Contains(ch)) wrongGuesses++;
        GuessEntry.Text = string.Empty;
        GuessEntry.Focus();
        UpdateDisplay();
    }
}

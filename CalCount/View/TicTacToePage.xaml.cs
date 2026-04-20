using System;
using Microsoft.Maui.Controls;

namespace CalCount.View;

public partial class TicTacToePage : ContentPage
{
    Button[,] buttons = new Button[3,3];
    bool xTurn = true;

    public TicTacToePage()
    {
        InitializeComponent();
        BuildBoard();
    }

    void BuildBoard()
    {
        BoardGrid.Children.Clear();
        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                var btn = new Button { FontSize = 32 };
                btn.Clicked += OnCellClicked;
                buttons[r,c] = btn;
                BoardGrid.Add(btn, c, r);
            }
        }
        UpdateStatus();
    }

    void OnCellClicked(object? sender, EventArgs e)
    {
        if (sender is Button b && string.IsNullOrEmpty(b.Text))
        {
            b.Text = xTurn ? "X" : "O";
            xTurn = !xTurn;
            UpdateStatus();
            var winner = CheckWinner();
            if (winner != null)
            {
                DisplayAlert("Game Over", $"{winner} wins!", "OK");
            }
            else if (IsBoardFull())
            {
                DisplayAlert("Game Over", "Draw!", "OK");
            }
        }
    }

    void UpdateStatus()
    {
        StatusLabel.Text = xTurn ? "X's turn" : "O's turn";
    }

    string? CheckWinner()
    {
        for (int i = 0; i < 3; i++)
        {
            if (!string.IsNullOrEmpty(buttons[i,0].Text) && buttons[i,0].Text == buttons[i,1].Text && buttons[i,1].Text == buttons[i,2].Text)
                return buttons[i,0].Text;
            if (!string.IsNullOrEmpty(buttons[0,i].Text) && buttons[0,i].Text == buttons[1,i].Text && buttons[1,i].Text == buttons[2,i].Text)
                return buttons[0,i].Text;
        }
        if (!string.IsNullOrEmpty(buttons[0,0].Text) && buttons[0,0].Text == buttons[1,1].Text && buttons[1,1].Text == buttons[2,2].Text)
            return buttons[0,0].Text;
        if (!string.IsNullOrEmpty(buttons[0,2].Text) && buttons[0,2].Text == buttons[1,1].Text && buttons[1,1].Text == buttons[2,0].Text)
            return buttons[0,2].Text;
        return null;
    }

    bool IsBoardFull()
    {
        foreach (var b in buttons) if (string.IsNullOrEmpty(b.Text)) return false;
        return true;
    }

    void OnResetClicked(object sender, EventArgs e)
    {
        xTurn = true;
        BuildBoard();
    }
}

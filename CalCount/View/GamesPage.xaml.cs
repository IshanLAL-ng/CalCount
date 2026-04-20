using System;
using Microsoft.Maui.Controls;

namespace CalCount.View;

public partial class GamesPage : ContentPage
{
    public GamesPage()
    {
        InitializeComponent();
    }

    private async void OnHangmanClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HangmanPage());
    }

    private async void OnPongClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PongPage());
    }

    private async void OnTicTacToeClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TicTacToePage());
    }
}

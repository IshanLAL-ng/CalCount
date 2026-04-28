namespace CalCount.View;

public partial class DashboardPage : ContentPage
{
    public DashboardPage(string username)
    {
        InitializeComponent();
        WelcomeLabel.Text = $"Welcome, {username}!";
    }
}

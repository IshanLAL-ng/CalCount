namespace CalCount.View;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = new ViewModel.LoginViewModel();
    }

}

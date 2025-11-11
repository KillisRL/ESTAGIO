using BarbeariaMatutosApp.ViewModels;
namespace BarbeariaMatutosApp.Views;

public partial class pgLoginBarbeiro : ContentPage
{
	public pgLoginBarbeiro(LoginViewModel loginViewModel)
	{
		InitializeComponent();

		BindingContext = loginViewModel;

    }
}
using BarbeariaMatutosApp.ViewModels;
namespace BarbeariaMatutosApp.Views;

public partial class pgPrincipal : ContentPage
{
	public pgPrincipal(PrincipalViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}
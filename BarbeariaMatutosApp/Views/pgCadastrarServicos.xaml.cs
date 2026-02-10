using BarbeariaMatutosApp.ViewModels;

namespace BarbeariaMatutosApp.Views;

public partial class pgCadastrarServicos : ContentPage
{
	public pgCadastrarServicos(CadastrarServicosViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
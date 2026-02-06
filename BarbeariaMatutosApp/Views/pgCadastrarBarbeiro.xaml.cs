using BarbeariaMatutosApp.ViewModels;

namespace BarbeariaMatutosApp.Views;

public partial class pgCadastrarBarbeiro : ContentPage
{
	public pgCadastrarBarbeiro(PerfilBarbeiroViewModel usuariosViewModel)
	{
		InitializeComponent();

		BindingContext = usuariosViewModel;

    }
}
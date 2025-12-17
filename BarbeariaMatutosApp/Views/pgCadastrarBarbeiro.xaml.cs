using BarbeariaMatutosApp.ViewModels;

namespace BarbeariaMatutosApp.Views;

public partial class pgCadastrarBarbeiro : ContentPage
{
	public pgCadastrarBarbeiro(UsuariosViewModel usuariosViewModel)
	{
		InitializeComponent();

		BindingContext = usuariosViewModel;

    }
}
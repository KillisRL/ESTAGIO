using BarbeariaMatutosApp.ViewModels;
namespace BarbeariaMatutosApp.Views;


public partial class pgConfiguracoes : ContentPage
{
	public pgConfiguracoes(UsuariosViewModel usuarios)
	{
		InitializeComponent();

		BindingContext = usuarios;
	}
}
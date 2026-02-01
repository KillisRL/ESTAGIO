using UsersDomain;
using BarbeariaMatutosApp.Services;
using BarbeariaMatutosApp.ViewModels;
using System.Net.Http.Json;
using UsersDomain.Entidades;

namespace BarbeariaMatutosApp.Views;

public partial class pgUsuarioCadastro : ContentPage
{
    public pgUsuarioCadastro(UsuariosViewModel usuarios)
	{

        InitializeComponent();
        BindingContext = usuarios;
    }
}
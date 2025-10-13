using Microsoft.Maui.Controls;
using System.Net.Http.Json;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using UsersDomain.Entidades;
using BarbeariaMatutosApp.ViewModels;

namespace BarbeariaMatutosApp.Views;

public partial class pgAgendamentoServico : ContentPage
{

    public pgAgendamentoServico(AgendamentoServicoViewModel  agendamentoServicoViewModel)
	{

        InitializeComponent();
        // Conecta a View (essa página) com a ViewModel
        BindingContext = agendamentoServicoViewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AgendamentoServicoViewModel viewModel)
        {
            await viewModel.LoadServicosCommand.ExecuteAsync(null);
        }
    }

    private void btnAgendar_Clicked(object sender, EventArgs e)
    {

    }
}
using BarbeariaMatutosApp.ViewModels;
namespace BarbeariaMatutosApp.Views;

public partial class pgFinalizarAgendamento : ContentPage
{

    public pgFinalizarAgendamento(FinalizarAgendamentoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    // ADICIONE ESTE M�TODO INTEIRO ABAIXO DO CONSTRUTOR
    protected override async void OnAppearing()
    {
        base.OnAppearing(); // � uma boa pr�tica chamar a implementa��o base

        // Pega a ViewModel que est� no BindingContext
        var viewModel = BindingContext as FinalizarAgendamentoViewModel;

        // Verifica se a ViewModel n�o � nula e se o comando pode ser executado
        if (viewModel != null && viewModel.LoadBarbeirosCommand.CanExecute(null))
        {
            // Executa o comando
            await viewModel.LoadBarbeirosCommand.ExecuteAsync(null);
        }
    }
}
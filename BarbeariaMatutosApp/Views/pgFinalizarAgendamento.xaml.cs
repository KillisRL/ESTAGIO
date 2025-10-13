using BarbeariaMatutosApp.ViewModels;
namespace BarbeariaMatutosApp.Views;

public partial class pgFinalizarAgendamento : ContentPage
{

    public pgFinalizarAgendamento(FinalizarAgendamentoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    // ADICIONE ESTE MÉTODO INTEIRO ABAIXO DO CONSTRUTOR
    protected override async void OnAppearing()
    {
        base.OnAppearing(); // É uma boa prática chamar a implementação base

        // Pega a ViewModel que está no BindingContext
        var viewModel = BindingContext as FinalizarAgendamentoViewModel;

        // Verifica se a ViewModel não é nula e se o comando pode ser executado
        if (viewModel != null && viewModel.LoadBarbeirosCommand.CanExecute(null))
        {
            // Executa o comando
            await viewModel.LoadBarbeirosCommand.ExecuteAsync(null);
        }
    }
}
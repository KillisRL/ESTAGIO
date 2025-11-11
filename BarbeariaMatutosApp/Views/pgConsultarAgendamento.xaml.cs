using BarbeariaMatutosApp.ViewModels;
namespace BarbeariaMatutosApp.Views;

public partial class pgConsultarAgendamento : ContentPage
{
	public pgConsultarAgendamento(ConsultarAgendamentoViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ConsultarAgendamentoViewModel viewModel)
        {
            await viewModel.CarregarAgendamentosCommand.ExecuteAsync(null);
        }
    }
}
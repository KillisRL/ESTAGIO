using BarbeariaMatutosApp.ViewModels;

namespace BarbeariaMatutosApp.Views;

public partial class pgConsultarServicos : ContentPage
{
	public pgConsultarServicos(ConsultarServicosViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ConsultarServicosViewModel viewModel)
        {
            await viewModel.ConsultarServicosCommand.ExecuteAsync(null);
        }
    }
}
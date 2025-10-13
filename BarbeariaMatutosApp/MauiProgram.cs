using BarbeariaMatutosApp.Services;
using BarbeariaMatutosApp.ViewModels;
using BarbeariaMatutosApp.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace BarbeariaMatutosApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		//SERVIÇOS
		builder.Services.AddSingleton<ApiServices>();
		//VIEWMODELS
        builder.Services.AddTransient<FinalizarAgendamentoViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<AgendamentoServicoViewModel>();

		//VIEWS
        builder.Services.AddTransient<pgFinalizarAgendamento>();
        builder.Services.AddTransient<pgUsuarioCadastro>();
        builder.Services.AddTransient<pgAgendamentoServico>();


#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

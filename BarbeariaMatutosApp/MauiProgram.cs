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
        builder.Services.AddSingleton<HttpClient>();
        //VIEWMODELS
        builder.Services.AddTransient<FinalizarAgendamentoViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<AgendamentoServicoViewModel>();
        builder.Services.AddSingleton<ConsultarAgendamentoViewModel>();
        builder.Services.AddTransient<PrincipalViewModel>();
        builder.Services.AddTransient<ConsultarServicosViewModel>();
        builder.Services.AddTransient<UsuariosViewModel>();

        //VIEWS
        builder.Services.AddTransient<pgFinalizarAgendamento>();
        builder.Services.AddTransient<pgUsuarioCadastro>();
        builder.Services.AddTransient<pgAgendamentoServico>();
        builder.Services.AddSingleton<pgConsultarAgendamento>();
        builder.Services.AddTransient<pgPrincipal>();
        builder.Services.AddTransient<pgLoginBarbeiro>();
        builder.Services.AddTransient<pgConsultarServicos>();
        builder.Services.AddTransient<pgCadastrarBarbeiro>();



#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

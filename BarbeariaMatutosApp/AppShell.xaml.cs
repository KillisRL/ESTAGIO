using BarbeariaMatutosApp.Views;

namespace BarbeariaMatutosApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(pgFinalizarAgendamento), typeof(pgFinalizarAgendamento));
            Routing.RegisterRoute(nameof(pgUsuarioCadastro), typeof(pgUsuarioCadastro));
            //Routing.RegisterRoute(nameof(pgAgendamentoServico), typeof(pgAgendamentoServico));
            //Routing.RegisterRoute(nameof(pgLoginBarbearia), typeof(pgLoginBarbearia));
            //Routing.RegisterRoute(nameof(pgPrincipal), typeof(pgPrincipal));
            Routing.RegisterRoute(nameof(pgLoginBarbeiro), typeof(pgLoginBarbeiro));
            Routing.RegisterRoute(nameof(pgConsultarAgendamento), typeof(pgConsultarAgendamento));
            Routing.RegisterRoute(nameof(pgConsultarServicos), typeof(pgConsultarServicos));
            Routing.RegisterRoute(nameof(pgCadastrarBarbeiro), typeof(pgCadastrarBarbeiro));
            //Routing.RegisterRoute(nameof(pgConfiguracoes), typeof(pgConfiguracoes));

        }
    }
}

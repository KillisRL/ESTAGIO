﻿using BarbeariaMatutosApp.Views;

namespace BarbeariaMatutosApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(pgFinalizarAgendamento), typeof(pgFinalizarAgendamento));
            Routing.RegisterRoute(nameof(pgUsuarioCadastro), typeof(pgUsuarioCadastro));
            Routing.RegisterRoute(nameof(pgAgendamentoServico), typeof(pgAgendamentoServico));
            Routing.RegisterRoute(nameof(pgLoginBarbearia), typeof(pgLoginBarbearia));
        }
    }
}

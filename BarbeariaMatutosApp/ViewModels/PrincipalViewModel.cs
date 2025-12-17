using BarbeariaMatutosApp.Services;
using BarbeariaMatutosApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersDomain.Entidades;

namespace BarbeariaMatutosApp.ViewModels
{
    public partial class PrincipalViewModel : BaseViewModel
    {
        private readonly ApiServices _apiServices;

        TipoUsuario tipoUsuario;

        [ObservableProperty]
        private string? nome;

        [ObservableProperty]
        private int? idUsuario;

        [ObservableProperty]
        private string usuarioTipo;

        [ObservableProperty]
        private bool _isAdminVisible;

        [ObservableProperty]
        private bool _isClienteVisible;

        [ObservableProperty]
        private bool  _IsAdminOrBarbeiroVisible;

        public PrincipalViewModel (ApiServices apiServices)
        {
            _apiServices = apiServices;
            CarregarDadosUsuario();

            SessaoUsuarioService.OnSessaoChanged += CarregarDadosUsuario;
        }
        private void CarregarDadosUsuario()
        {

            var usuario = SessaoUsuarioService.Usuariologado;
            var barbeiro = SessaoUsuarioService.BarbeiroLogado;

            bool isAdmin = false;
            bool isBarbeiro = false;
            bool isCliente = false;

            if (usuario != null)
            {
                nome = usuario.Nome;
                idUsuario = usuario.IDUsuario;
                usuarioTipo = usuario.IdPessoaTipo.ToString();
                isCliente = usuario.IdPessoaTipo == TipoUsuario.Cliente;
            }

            else if(barbeiro != null)
            {
                nome = barbeiro.NomeBarbeiro;
                idUsuario = barbeiro.IdBarbeiro;
                usuarioTipo = barbeiro.IdPessoaTipo.ToString();

                isAdmin = barbeiro.IdPessoaTipo == TipoUsuario.Admin;
                isBarbeiro = barbeiro.IdPessoaTipo == TipoUsuario.Barbeiro;
            }
            else
            {
                nome = "Convidado";
                idUsuario = null;
                usuarioTipo = "Nenhum";
            }
            _isAdminVisible = isAdmin;
            _isClienteVisible = isCliente;
            _IsAdminOrBarbeiroVisible = isBarbeiro || isAdmin;
        }

        [RelayCommand]
        private async Task ConsultarAgendamento()
        {
            await Shell.Current.GoToAsync(nameof(pgConsultarAgendamento));
        }

        [RelayCommand]
        private async Task ConsultarServicos()
        {
           await Shell.Current.GoToAsync(nameof(pgConsultarServicos));
        }
    }
}

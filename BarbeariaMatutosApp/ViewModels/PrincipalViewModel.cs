using BarbeariaMatutosApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarbeariaMatutosApp.ViewModels
{
    public partial class PrincipalViewModel : BaseViewModel
    {
        private readonly ApiServices _apiServices;

        [ObservableProperty]
        private string? nome;

        [ObservableProperty]
        private int? idUsuario;

        public PrincipalViewModel()
        {
            CarregarDadosUsuario();
        }

        private void CarregarDadosUsuario()
        {
            var usuario = SessaoUsuarioService.Usuariologado;
            if(usuario != null)
            {
                nome = usuario.Nome;
                idUsuario = usuario.IDUsuario;
            }
        }
    }
}

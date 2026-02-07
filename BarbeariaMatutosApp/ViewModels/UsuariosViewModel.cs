using BarbeariaMatutosApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UsersDomain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersDomain.Entidades;
using System.Collections.ObjectModel;
using BarbeariaMatutosApp.Views;

namespace BarbeariaMatutosApp.ViewModels
{
    public partial class UsuariosViewModel : BaseViewModel
    {

        private readonly ApiServices _apiservices;


        public UsuariosViewModel(ApiServices apiServices)
        {
            _apiservices = apiServices;
         
        }


        [RelayCommand]
        private async Task AbrirTelaAlteracaoAsync()
        {
            bool confirmar = await Shell.Current.DisplayAlert("Atenção",
                $"Deseja realizar a alteração do Perfil?",
                "Sim", "Não");

            if (!confirmar)
                return; // Usuário cancelou a ação

            if (IsBarbeiro == true || IsAdmin == true)
            {
                Barbeiro barbeiroLogado = SessaoUsuarioService.BarbeiroLogado;
                if(barbeiroLogado != null)
                {
                    var ParametroNavegacao = new Dictionary<string, object>
                    {
                        {"BarbeiroParaEditar", barbeiroLogado }
                    };

                    await Shell.Current.GoToAsync(nameof(pgCadastrarBarbeiro), ParametroNavegacao);
                }

                else
                {
                    await Shell.Current.DisplayAlert("Erro", "Não foi possível recuperar os dados do barbeiro logado.", "OK");
                }
               

            }
            else
            {

                Users usuarioLogado = SessaoUsuarioService.Usuariologado;

                if(usuarioLogado != null)
                {
                    var parametroNavegacao = new Dictionary<string, object>
                    {
                        {"ClienteParaEditar", usuarioLogado }
                    };

                    await Shell.Current.GoToAsync(nameof(pgUsuarioCadastro), parametroNavegacao);
                }

                else
                {
                    await Shell.Current.DisplayAlert("Erro", "Não foi possível recuperar os dados do usuário logado.", "OK");
                }
            }
        }
    }
}

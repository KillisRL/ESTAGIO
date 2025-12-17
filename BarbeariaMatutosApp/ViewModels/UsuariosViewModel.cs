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
        // Use readonly para serviços injetados e não o deixe nulo
        private readonly ApiServices _apiservices;

        // Propriedades ligadas à tela (XAML)
        [ObservableProperty] private int idbarbeiro;
        [ObservableProperty] private string nomebarbeiro;
        [ObservableProperty] private bool ativo;
        [ObservableProperty] private string login;
        [ObservableProperty] private string senha;
        // Adicione a propriedade para o tipo de pessoa, se necessário
        [ObservableProperty] private TipoUsuario tipoUsuarioSelecionado; // Valor padrão, por exemplo

        // Lista para expor as opções do enum para a View
        public ObservableCollection<TipoUsuario> TiposUsuarioDisponiveis { get; }

        public UsuariosViewModel(ApiServices apiServices)
        {
            // Injeção de dependência correta
            _apiservices = apiServices;
            // Inicializa como ativo por padrão, se fizer sentido
            Ativo = true;

            // Inicializa a lista de opções com os valores do enum
            TiposUsuarioDisponiveis = new ObservableCollection<TipoUsuario>(Enum.GetValues(typeof(TipoUsuario)).Cast<TipoUsuario>());

            // Define um valor padrão para a seleção, por exemplo, Barbeiro
            tipoUsuarioSelecionado = TipoUsuario.Barbeiro;
        }

        [RelayCommand]
        private async Task CadastrarBarbeiroAsync()
        {
            // 1. Validação Básica (Opcional, mas recomendado)
            if (string.IsNullOrWhiteSpace(Nomebarbeiro) || string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Senha))
            {
                await Application.Current.MainPage.DisplayAlert("Atenção", "Preencha todos os campos obrigatórios.", "OK");
                return;
            }

            try
            {
                // 2. Cria o objeto usando os dados DAS PROPRIEDADES DA VIEWMODEL
                var barbeiroParaCadastrar = new Barbeiro
                {
                    
                    IdBarbeiro = this.idbarbeiro,
                    NomeBarbeiro = this.Nomebarbeiro, // Pega o valor da propriedade da classe
                    Ativo = this.Ativo,
                    Login = this.Login,
                    Senha = this.Senha,
                    IdPessoaTipo = this.tipoUsuarioSelecionado

                };

                if (tipoUsuarioSelecionado == TipoUsuario.Cliente)
                {
                    await Application.Current.MainPage.DisplayAlert("Atenção", "Nessa tela só é possível cadastrar usuários do tipo 'Admin' e 'Barbeiro'.", "OK");
                    return;
                }

                // 3. Envia o objeto preenchido para o serviço
                bool sucesso = await _apiservices.CadastrarBarbeiroAsync(barbeiroParaCadastrar);

                if (sucesso)
                {
                    await Application.Current.MainPage.DisplayAlert("Sucesso", "Barbeiro cadastrado com sucesso!", "OK");
                    LimparCampos(); // Método auxiliar para limpar a tela
                    await Shell.Current.GoToAsync(nameof(pgPrincipal));
                }
                else
                {
                    // O serviço já deve ter logado o erro, avise o usuário.
                    await Application.Current.MainPage.DisplayAlert("Erro", "Não foi possível realizar o cadastro. Verifique os dados.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro Crítico", $"Falha de comunicação: {ex.Message}", "OK");
            }
        }

        // Método para limpar os campos após o cadastro
        private void LimparCampos()
        {
            Nomebarbeiro = string.Empty;
            Login = string.Empty;
            Senha = string.Empty;
            Ativo = true; // Reseta para o padrão
        }
    }
}

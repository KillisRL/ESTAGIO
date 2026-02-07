using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using BarbeariaMatutosApp.Views;
using BarbeariaMatutosApp.Services;
using System.Threading.Tasks;
using UsersDomain.Entidades;
using CommunityToolkit.Mvvm.Input;

namespace BarbeariaMatutosApp.ViewModels
{
    [QueryProperty(nameof(BarbeiroRecebido), "BarbeiroParaEditar")]
    public partial class PerfilBarbeiroViewModel : BaseViewModel
    {
        //instacia da nossa API
        private readonly ApiServices _apiservices;

        //CONTROLE DE STATUS DA TELA QUANDO FOR ALTERAÇÃO E CRIAÇÃO
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TituloPagina))]
        [NotifyPropertyChangedFor(nameof(NomeBotaoAcao))]
        private AcaoTela _acaoTela;

        //propriedade para receber se é alteração ou cadastro para mudar o nome da tela
        public string TituloPagina => AcaoTela == AcaoTela.Cadastro ? "Novo Barbeiro" : "Editar Perfil";
        public string NomeBotaoAcao => AcaoTela == AcaoTela.Cadastro ? "Cadastrar" : "Salvar Alterações";

        // Propriedades ligadas à tela (XAML PARA BARBEIRO)
        [ObservableProperty] private int idbarbeiro;
        [ObservableProperty] private string nomebarbeiro;
        [ObservableProperty] private bool ativo;
        [ObservableProperty] private string login;
        [ObservableProperty] private string senha;
        [ObservableProperty] private TipoUsuario _tipoUsuario;

        [ObservableProperty] private TipoUsuario tipoUsuarioSelecionado;
        //Objeto populato pelos dados enviados pela tela passada
        [ObservableProperty] private Barbeiro? _barbeiroRecebido;

        //serve para identificar os usuário disponíveis em nosso sistema
        public ObservableCollection<TipoUsuario> TiposUsuarioDisponiveis { get; }

        public PerfilBarbeiroViewModel(ApiServices apiServices)
        {
            // Injeção de dependência correta
            _apiservices = apiServices;
            //carrega por default o campo como verdadeiro
            Ativo = true;

            TiposUsuarioDisponiveis = new ObservableCollection<TipoUsuario>(Enum.GetValues(typeof(TipoUsuario)).Cast<TipoUsuario>());

            if (AcaoTela == AcaoTela.Cadastro)
            {
                TipoUsuarioSelecionado = TipoUsuario.Barbeiro;
            }

        }

        partial void OnBarbeiroRecebidoChanged(Barbeiro? value)
        {
            if(value is not null)
            {
                AcaoTela = AcaoTela.Alteracao;

                // Preenche campos especificamente para Barbeiro
                Idbarbeiro = value.IdBarbeiro;
                Nomebarbeiro = value.NomeBarbeiro;
                Ativo = value.Ativo.Value;        
                Login = value.Login;  
                Senha = value.Senha;     
                TipoUsuario = value.IdPessoaTipo; 
            }
            else
            {
                ConfigurarModoCadastro();
            }
        }

        private void ConfigurarModoCadastro()
        {
            AcaoTela = AcaoTela.Cadastro;
            Nomebarbeiro = string.Empty;
            Ativo = true;
            Login = string.Empty;
            Senha = string.Empty;
            TipoUsuario = TipoUsuario.Barbeiro;
        }


        [RelayCommand]
        private async Task SalvarAsync()
        {
            if (string.IsNullOrWhiteSpace(Nomebarbeiro) || string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Senha))
            {
                await Application.Current.MainPage.DisplayAlert("Atenção", "Preencha todos os campos obrigatórios.", "OK");
                return;
            }
            if(AcaoTela == AcaoTela.Alteracao)
            {
                AlterarCadastroBarbeiro();
            }

            else
            {
                CadastrarBarbeiroAsync();
            }

        }



        [RelayCommand]
        private async Task CadastrarBarbeiroAsync()
        {
            try
            {
                // 2. Cria o objeto usando os dados DAS PROPRIEDADES DA VIEWMODEL
                var barbeiroParaCadastrar = new BarbeiroCreate
                {

                    NomeBarbeiro = this.Nomebarbeiro, // Pega o valor da propriedade da classe
                    Ativo = this.Ativo,
                    Login = this.Login,
                    Senha = this.Senha,
                    IdPessoaTipo = this.TipoUsuarioSelecionado

                };

                if (TipoUsuario == TipoUsuario.Cliente)
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

        [RelayCommand]
        private async Task AlterarCadastroBarbeiro()
        {
            try
            {
                var alterarBarbeiro = new Barbeiro
                {
                    IdBarbeiro = this.idbarbeiro,
                    NomeBarbeiro = this.Nomebarbeiro,
                    Login = this.Login,
                    Ativo = this.Ativo,
                    Senha = this.Senha,
                    IdPessoaTipo = this.TipoUsuario
                };
                bool sucesso = await _apiservices.AlterarBarbeiroAsync(alterarBarbeiro);

                if (sucesso)
                {
                    await Application.Current.MainPage.DisplayAlert("Sucesso", "Alteração realizada com sucesso!", "OK");
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
    }
}

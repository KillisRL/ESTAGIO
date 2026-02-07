using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BarbeariaMatutosApp.Services;
using System.Threading.Tasks;
using BarbeariaMatutosApp.Views;
using UsersDomain.Entidades;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace BarbeariaMatutosApp.ViewModels
{
    [QueryProperty(nameof(ClienteRecebido), "ClienteParaEditar")]
    public partial class PerfilClienteViewModel : BaseViewModel
    {

        private readonly ApiServices _apiservices;

        //CONTROLE DE STATUS DA TELA QUANDO FOR ALTERAÇÃO E CRIAÇÃO
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TituloPagina))]
        [NotifyPropertyChangedFor(nameof(NomeBotaoAcao))]
        private AcaoTela _acaoTela;

        //propriedade para receber se é alteração ou cadastro para mudar o nome da tela
        public string TituloPagina => AcaoTela == AcaoTela.Cadastro ? "Novo Cliente" : "Editar Perfil";
        public string NomeBotaoAcao => AcaoTela == AcaoTela.Cadastro ? "Cadastrar" : "Salvar Alterações";


        //PROPRIEDADES DA TELA PARA OS CLIENTES
        [ObservableProperty] private string nome;
        [ObservableProperty] private int idUsuario;
        [ObservableProperty] private string email;
        [ObservableProperty] private string telefone;
        [ObservableProperty] private string senhahash;
        [ObservableProperty] private TipoUsuario _tipoUsuario;


        [ObservableProperty] private Users? _clienteRecebido;
        
        public PerfilClienteViewModel(ApiServices apiServices)
        {
            _apiservices = apiServices;
            // Inicializa como ativo por padrão, se fizer sentido


        }
        partial void OnClienteRecebidoChanged(Users? value)
        {
            if (value is not null)
            {

                AcaoTela = AcaoTela.Alteracao;

                // Preenche campos especificamente para Barbeiro
                IdUsuario = value.IDUsuario;   // <--- Corrigido (era idUsuario)
                Nome = value.Nome;             // <--- Corrigido (era nome)
                Senhahash = value.SenhaHash;   // <--- Corrigido
                Telefone = value.Telefone;     // <--- Corrigido
                TipoUsuario = value.IdPessoaTipo; // <--- Corrigido
                Email = value.Email;
                // Senha geralmente não se preenche por segurança
            }
            else
            {
                ConfigurarModoCadastro();
            }
        }

        private void ConfigurarModoCadastro()
        {
            AcaoTela = AcaoTela.Cadastro;
            Nome = string.Empty;
            Email = string.Empty;
            Telefone = string.Empty;
            Senhahash = string.Empty;
            TipoUsuario = TipoUsuario.Cliente;
        }

        [RelayCommand]
        private async Task SalvarAsync()
        {
            if (string.IsNullOrEmpty(Nome) || string.IsNullOrEmpty(Senhahash))
            {
                await Application.Current.MainPage.DisplayAlert("Atenção", "Preencha todos os campos obrigatórios.", "OK");
                return;
            }

            if(AcaoTela == AcaoTela.Alteracao)
            {
                // Pede confirmação ao usuário
                bool confirmar = await Shell.Current.DisplayAlert("Confirmar Cancelamento",
                    $"Tem certeza que deseja realizar a alteração no seu perfil?",
                    "Sim", "Não");
                if (!confirmar)
                    return;
                await ExecutarAlteracao();
            }
            else
            {


                await ExecutarCadastro();
            }
        }

        private async Task ExecutarCadastro()
        {
            try
            {
                var clienteParaCadastrar = new UserCreate
                {
                    Nome = this.Nome,
                    Email = this.Email,
                    SenhaHash = this.Senhahash,
                    Telefone = this.Telefone,
                    IdPessoaTipo = TipoUsuario.Cliente
                };

                bool sucesso = await _apiservices.CadastrarClienteAsync(clienteParaCadastrar);
                if (sucesso)
                {
                    await Application.Current.MainPage.DisplayAlert("Sucesso", "Cliente cadastrado com sucesso!", "OK");
                    await Shell.Current.GoToAsync(nameof(pgLoginBarbearia));
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


        private async Task ExecutarAlteracao()
        {
            try
            {
                var clienteParaAlterar = new Users
                {
                    IDUsuario = this.idUsuario,
                    Nome = this.Nome,
                    Email = this.Email,
                    SenhaHash = this.Senhahash,
                    Telefone = this.Telefone,
                    IdPessoaTipo = TipoUsuario.Cliente
                };
                bool sucesso = await _apiservices.AlterarClienteAsync(clienteParaAlterar);

                if (sucesso) {
                    await Application.Current.MainPage.DisplayAlert("Sucesso", "Cliente Alterado com Sucesso!", "OK");
                    await Shell.Current.GoToAsync(nameof(pgConfiguracoes));
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erro Crítico", "Não foi possível realizar a alteração do seu perfil", "OK");
                }
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro Crítico", $"Falha de comunicação: {ex.Message}", "OK");
            }
        }
    }
}

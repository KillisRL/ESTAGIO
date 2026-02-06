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

        public string TituloPagina => _acaoTela == AcaoTela.Cadastro
       


        //PROPRIEDADES DA TELA PARA OS CLIENTES
        [ObservableProperty] private string nome;
        [ObservableProperty] private int idUsuario;
        [ObservableProperty] private string email;
        [ObservableProperty] private string telefone;
        [ObservableProperty] private string senhahash;
        [ObservableProperty] private TipoUsuario _tipoUsuario;



        //Tipo de Usuário que será identificado.
        [ObservableProperty]
        private string usuarioTipo;

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
                // Preenche campos especificamente para Barbeiro
                idUsuario = value.IDUsuario;
                nome = value.Nome;
                senhahash = value.SenhaHash;
                telefone = value.Telefone;
                TipoUsuario = value.IdPessoaTipo;
                email = value.Email;
                // Senha geralmente não se preenche por segurança
            }
        }

        [RelayCommand]
        private async Task CadastrarClienteAsync()
        {
            if (string.IsNullOrEmpty(Nome) || string.IsNullOrEmpty(Senhahash))
            {
                await Application.Current.MainPage.DisplayAlert("Atenção", "Preencha todos os campos obrigatórios.", "OK");
                return;
            }

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
    }
}

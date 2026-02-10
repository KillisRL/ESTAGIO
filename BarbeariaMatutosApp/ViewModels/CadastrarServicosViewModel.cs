using BarbeariaMatutosApp.Services;
using BarbeariaMatutosApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using UsersDomain.Entidades;

namespace BarbeariaMatutosApp.ViewModels
{
    public partial class CadastrarServicosViewModel : BaseViewModel
    {
        [ObservableProperty] private string descServico;
        [ObservableProperty] private string duracaoServico;
        [ObservableProperty] private decimal valorServico;


        private readonly ApiServices _apiServices;

        public CadastrarServicosViewModel(ApiServices apiServices)
        {
            _apiServices = apiServices;
        }

        [RelayCommand]
        private async Task CadastrarServicosAsync()
        {
            try
            {
                
                var servicoParaCadastrar = new Servicos
                {
                    DescServico = this.descServico, 
                    Duracao = this.duracaoServico,
                    ValorServico = this.valorServico


                };

                // 3. Envia o objeto preenchido para o serviço
                bool sucesso = await _apiServices.CadastrarServicosAsync(servicoParaCadastrar);

                if (sucesso)
                {
                    await Application.Current.MainPage.DisplayAlert("Sucesso", "Serviço cadastrado com sucesso!", "OK");
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
            DescServico = string.Empty;
            DuracaoServico = string.Empty;
            ValorServico = 0;
        }
    }
}

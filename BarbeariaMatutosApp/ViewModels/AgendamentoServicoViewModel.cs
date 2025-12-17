using BarbeariaMatutosApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using UsersDomain.Entidades;
using BarbeariaMatutosApp.Services;
namespace BarbeariaMatutosApp.ViewModels
{
    public partial class AgendamentoServicoViewModel : ObservableObject
    {
        private readonly ApiServices _apiServices;

        [ObservableProperty]
        private string? nome;

        [ObservableProperty]
        private int? idUsuario;

        [ObservableProperty]
        private ObservableCollection<Servicos> servicos;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private Servicos? servicoSelecionado;

        // Construtor recebe apenas o ApiServices
        public AgendamentoServicoViewModel(ApiServices apiServices)
        {
            _apiServices = apiServices;
            Servicos = new ObservableCollection<Servicos>();
            CarregarDadosUsuario();
        }

        private void CarregarDadosUsuario()
        {
            var usuario = SessaoUsuarioService.Usuariologado;
            if (usuario != null)
            {
                Nome = usuario.Nome;
                IdUsuario = usuario.IDUsuario;
            }
        }

        [RelayCommand]
        private async Task LoadServicosAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                // CHAMA O SEU SERVIÇO (que já tem a URL correta para Windows/Android)
                var servicosDaApi = await _apiServices.GetServicosAsync();

                if (servicosDaApi != null)
                {
                    Servicos.Clear();
                    foreach (var servico in servicosDaApi)
                    {
                        Servicos.Add(servico);
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", $"Não foi possível carregar os serviços: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task AgendarServicoAsync()
        {
            if (ServicoSelecionado != null)
            {
                var navigationParameter = new Dictionary<string, object>
                {
                    { "Servico", ServicoSelecionado }
                };

                await Shell.Current.GoToAsync(nameof(pgFinalizarAgendamento), navigationParameter);
            }
        }
    }
}

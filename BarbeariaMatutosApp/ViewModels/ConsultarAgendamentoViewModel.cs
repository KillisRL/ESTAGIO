using BarbeariaMatutosApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UsersDomain.Entidades;

namespace BarbeariaMatutosApp.ViewModels
{
    public partial class ConsultarAgendamentoViewModel : ObservableObject
    {
        private readonly ApiServices _apiServices;


        [ObservableProperty]
        private ObservableCollection<AgendamentoDTO> agendamentos;


        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy;

        public bool IsNotBusy => !isBusy;

        public ConsultarAgendamentoViewModel(ApiServices apiServices)
        {
            _apiServices = apiServices;
            agendamentos = new ObservableCollection<AgendamentoDTO>();
        }

        [RelayCommand]
        private async Task CarregarAgendamento()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                var agendamentoapi = await _apiServices.GetAgendamentoAsync();

                if(agendamentoapi?.Any() == true)
                {
                    agendamentos.Clear();
                    foreach (var agendamento in agendamentoapi)
                    {
                        Agendamentos.Add(agendamento);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro no ViewModel: {ex.Message}");
                await Shell.Current.DisplayAlert("Erro", "Não foi possível carregar os agendamentos", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}

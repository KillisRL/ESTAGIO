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
        private async Task CarregarAgendamentosAsync() 
        {
            if (IsBusy) return;
            int? userId = SessaoUsuarioService.Usuariologado?.IDUsuario;

            if (!userId.HasValue) 
            {
                Debug.WriteLine("Usuário não logado, não é possível carregar agendamentos.");
                await Shell.Current.DisplayAlert("Erro", "Você precisa estar logado para ver seus agendamentos.", "OK");
                return; 
            }
            try
            {
                IsBusy = true;
                // Chama o método da ApiService passando o ID
                var agendamentoapi = await _apiServices.GetMeusAgendamentosAsync(userId.Value);

                if (agendamentoapi?.Any() == true)
                {
                    Agendamentos.Clear();
                    foreach (var agendamento in agendamentoapi)
                    {
                        Agendamentos.Add(agendamento);
                    }
                }
                else
                {
                    Agendamentos.Clear(); 
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}

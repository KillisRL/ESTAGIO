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
        private AgendamentoDTO? agendamentoSelecionado;

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
        [RelayCommand]
        private async Task CancelarAgendamentoAsync() // Nome corrigido
        {
            // Verifica se há um agendamento selecionado
            if (agendamentoSelecionado == null)
            {
                await Shell.Current.DisplayAlert("Atenção", "Selecione um agendamento para cancelar.", "OK");
                return;
            }
            if(agendamentoSelecionado.IdSituacao == 3 || agendamentoSelecionado.IdSituacao ==2)
            {
                await Shell.Current.DisplayAlert("Atenção", "Só é possível cancelar serviços na situação 'Aberto'.", "OK");
                return;
            }

            // Pede confirmação ao usuário
            bool confirmar = await Shell.Current.DisplayAlert("Confirmar Cancelamento",
                $"Tem certeza que deseja cancelar o agendamento {agendamentoSelecionado.IdAgendamento} do dia {agendamentoSelecionado.DataHora:dd/MM/yyyy HH:mm}?",
                "Sim", "Não");

            if (!confirmar)
                return; // Usuário cancelou a ação

            try
            {
                // Pega o ID (sem .Value)
                int idAgendamento = agendamentoSelecionado.IdAgendamento;

                // Chama a API e armazena o resultado booleano
                bool sucesso = await _apiServices.CancelarAgendamentoAsync(idAgendamento);

                // Verifica o resultado da operação
                if (sucesso)
                {
                    await Shell.Current.DisplayAlert("Sucesso", "Agendamento cancelado.", "OK");

                    await CarregarAgendamentosAsync(); 

                    agendamentoSelecionado = null; // Limpa a seleção
                }
                else
                {
                    await Shell.Current.DisplayAlert("Erro", "Não foi possível cancelar o agendamento. Tente novamente.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao cancelar agendamento: {ex.Message}");
                await Shell.Current.DisplayAlert("Erro", "Ocorreu um erro inesperado ao tentar cancelar.", "OK");
            }
        }
    }
}

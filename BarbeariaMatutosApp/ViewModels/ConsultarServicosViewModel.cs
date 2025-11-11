using CommunityToolkit.Mvvm.ComponentModel;
using System;
using BarbeariaMatutosApp.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersDomain.Entidades;
using BarbeariaMatutosApp.Views;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;

namespace BarbeariaMatutosApp.ViewModels
{

    public partial class ConsultarServicosViewModel : BaseViewModel
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

        public ConsultarServicosViewModel(ApiServices apiServices)
        {
            _apiServices = apiServices;
            Agendamentos = new ObservableCollection<AgendamentoDTO>();
        }

        [RelayCommand]
        private async Task ConsultarServicosAsync()
        {
            if (IsBusy) return;
            int? IdBarbeiro = SessaoUsuarioService.BarbeiroLogado?.IdBarbeiro;

            if(!IdBarbeiro.HasValue)
            {
                Debug.WriteLine("Barbeiro não logado, não é possível carregar os serviços.");
                await Shell.Current.DisplayAlert("Erro", "Você precisa estar logado para ver seus agendamentos.", "OK");
                return;
            }
            try
            {
                IsBusy = true;
                var servicosAPI = await _apiServices.GetMeusServicosAsync(IdBarbeiro.Value);
                if(servicosAPI?.Any() ==true)
                {
                    Agendamentos.Clear();
                    foreach(var servicos in servicosAPI)
                    {
                        Agendamentos.Add(servicos);
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
        private async Task CancelarServicosAsync() // Nome corrigido
        {
            // Verifica se há um agendamento selecionado
            if (agendamentoSelecionado == null)
            {
                await Shell.Current.DisplayAlert("Atenção", "Selecione um agendamento para cancelar.", "OK");
                return;
            }
            if (agendamentoSelecionado.IdSituacao == 3 || agendamentoSelecionado.IdSituacao == 2)
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

                    await ConsultarServicosAsync();

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

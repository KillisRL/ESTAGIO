using System.Windows.Input;
using CommunityToolkit.Mvvm.Input; // Para AsyncRelayCommand e AsyncRelayCommand<T>
using Microsoft.Maui.Controls;
using System.Diagnostics;
// using System.Security.AccessControl; // Este using parece desnecessário aqui

namespace BarbeariaMatutosApp.ViewModels
{
    // Certifique-se que BaseNotifyViewModel herda de ObservableObject
    public partial class BaseViewModel : BaseNotifyViewModel
    {
        // Propriedades ICommand devem ser readonly se inicializadas no construtor
        public ICommand VoltarCommand { get; }
        public ICommand AbrirViewCommand { get; }

        public BaseViewModel()
        {
            VoltarCommand = new AsyncRelayCommand(VoltarAsync); // Associa ao método de instância
            AbrirViewCommand = new AsyncRelayCommand<Type>(AbrirViewAsync); // Associa ao método de instância
        }

        // REMOVA 'static' daqui
        private async Task VoltarAsync()
        {
            if (Application.Current?.MainPage?.Navigation.NavigationStack.Count > 1)
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            }
        }

        // REMOVA 'static' daqui
        private async Task AbrirViewAsync(Type pageType)
        {
            if (pageType == null)
            {
                Debug.WriteLine("Erro: Tipo de página para navegação é nulo.");
                return;
            }

            try
            {
                // Usa Injeção de Dependência para criar a página
                var page = App.Current.Handler.MauiContext.Services.GetService(pageType) as ContentPage;

                if (page != null)
                {
                    // Acessa a Navigation a partir da MainPage atual
                    await Application.Current.MainPage.Navigation.PushAsync(page);
                }
                else
                {
                    Debug.WriteLine($"Erro: Não foi possível resolver a página do tipo {pageType.Name}. Verifique o registro no MauiProgram.cs.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao navegar para {pageType.Name}: {ex.Message}");
            }
        }
    }
}
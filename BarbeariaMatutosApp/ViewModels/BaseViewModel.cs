using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using UsersDomain.Entidades; // Certifique-se que TipoUsuario está aqui
using BarbeariaMatutosApp.Services;
using BarbeariaMatutosApp.Views;
using CommunityToolkit.Mvvm.ComponentModel; // Para ObservableObject

namespace BarbeariaMatutosApp.ViewModels
{
    public partial class BaseNotifyViewModel : ObservableObject
    {

    }

    public partial class BaseViewModel : BaseNotifyViewModel, IDisposable
    {
        // Comandos de navegação
        public ICommand VoltarCommand { get; }
        public ICommand AbrirViewCommand { get; }

        // Propriedades de conveniência para verificar o tipo de usuário
        public bool IsAdmin => SessaoUsuarioService.Usuariologado?.IdPessoaTipo == TipoUsuario.Admin;
        public bool IsCliente => SessaoUsuarioService.Usuariologado?.IdPessoaTipo == TipoUsuario.Cliente;
        public bool IsBarbeiro => SessaoUsuarioService.BarbeiroLogado != null; // Se BarbeiroLogado não for nulo, é um barbeiro
        public bool IsNaoBarbeiro => IsAdmin || IsCliente;

        public BaseViewModel()
        {
            // Inicializa comandos
            VoltarCommand = new AsyncRelayCommand(VoltarAsync);
            AbrirViewCommand = new AsyncRelayCommand<Type>(AbrirViewAsync);

            // Inscreve-se em mudanças na sessão para atualizar a UI
            SessaoUsuarioService.OnSessaoChanged += NotificarMudancaDeSessao;
        }

        // Método para o comando Voltar
        private async Task VoltarAsync()
        {
            // Shell.Current.GoToAsync("..") é geralmente mais simples com Shell
            if (Application.Current?.MainPage is Shell shell)
            {
                await shell.GoToAsync("..");
            }
            else if (Application.Current?.MainPage?.Navigation.NavigationStack.Count > 1)
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            }
        }

        // Método para o comando AbrirView
        private async Task AbrirViewAsync(Type pageType)
        {
            if (pageType == null)
            {
                Debug.WriteLine("Erro: Tipo de página para navegação é nulo.");
                return;
            }

            try
            {
                // Verifica se a página atual já é a página de destino (para evitar empilhar a mesma página)
                // Isso pode ser útil para rotas simples, mas com o Shell você pode usar rotas absolutas
                // ou verificar o stack do Shell.

                // Melhor usar o Shell para navegação, se você estiver usando Shell
                if (Application.Current?.MainPage is Shell shell)
                {
                    // Usa o nome da rota registrado no AppShell
                    await shell.GoToAsync(pageType.Name);
                }
                else
                {
                    // Fallback para navegação tradicional se não for Shell ou em outro contexto
                    var page = App.Current.Handler.MauiContext.Services.GetService(pageType) as ContentPage;
                    if (page != null)
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(page);
                    }
                    else
                    {
                        Debug.WriteLine($"Erro: Não foi possível resolver a página do tipo {pageType.Name}. Verifique o registro no MauiProgram.cs.");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao navegar para {pageType.Name}: {ex.Message}");
            }
        }

        // Método chamado quando a sessão muda para notificar a UI
        private void NotificarMudancaDeSessao()
        {
            // Avisa a UI para reavaliar todas as propriedades de permissão
            OnPropertyChanged(nameof(IsAdmin));
            OnPropertyChanged(nameof(IsCliente));
            OnPropertyChanged(nameof(IsBarbeiro));
            OnPropertyChanged(nameof(IsNaoBarbeiro));
        }

        // Limpa a inscrição do evento quando a ViewModel é descartada
        public void Dispose()
        {
            SessaoUsuarioService.OnSessaoChanged -= NotificarMudancaDeSessao;
            // GC.SuppressFinalize(this); // Geralmente não é necessário em classes não gerenciadas
        }

        [RelayCommand]
        private async Task FazerLogoutAsync()
        {
            // 1. Pergunta de confirmação (boa prática de UX)
            bool confirmar = await Shell.Current.DisplayAlert("Sair",
                                                              "Deseja realmente sair do sistema?",
                                                              "Sim", "Não");
            if (!confirmar)
                return;

            // 2. Chama o serviço para limpar os dados
            SessaoUsuarioService.Logout();

            // 3. Navegação Crítica: Usando "//" (Absolute Routing)
            // Isso é MUITO importante. Usar "//" limpa a pilha de navegação.
            // O usuário não conseguirá voltar para a tela anterior apertando "Voltar".
            await Shell.Current.GoToAsync(nameof(pgLoginBarbearia));
        }
    }
}
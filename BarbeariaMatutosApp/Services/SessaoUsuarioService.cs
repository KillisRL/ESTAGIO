using UsersDomain;
using UsersDomain.Entidades;

namespace BarbeariaMatutosApp.Services
{
    public static class SessaoUsuarioService
    {
        public static Users? Usuariologado { get; private set; } // Mude para private set
        public static Barbeiro? BarbeiroLogado { get; private set; } // Mude para private set

        // Evento que a BaseViewModel vai "ouvir"
        public static event Action? OnSessaoChanged;

        public static void IniciarSessao(Users users)
        {
            Usuariologado = users;
            BarbeiroLogado = null; // Garante que só um esteja logado
            OnSessaoChanged?.Invoke(); // Dispara o evento de mudança
        }

        public static void IniciarSessaoBarbeiro(Barbeiro barbeiro)
        {
            BarbeiroLogado = barbeiro;
            Usuariologado = null; // Garante que só um esteja logado
            OnSessaoChanged?.Invoke(); // Dispara o evento de mudança
        }

        public static void EncerrarSessao() // Método de Logout unificado
        {
            Usuariologado = null;
            BarbeiroLogado = null;
            OnSessaoChanged?.Invoke(); // Dispara o evento de mudança
        }

        public static void Logout()
        {
            Usuariologado = null;
            BarbeiroLogado = null;
        }

        public static bool IsLogado => Usuariologado != null || BarbeiroLogado != null;
    }
}

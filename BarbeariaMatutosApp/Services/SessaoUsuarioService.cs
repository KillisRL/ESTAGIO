using UsersDomain;
using UsersDomain.Entidades;

namespace BarbeariaMatutosApp.Services
{
    public static class SessaoUsuarioService
    {
        public static Users Usuariologado { get; set; }
        public static void IniciarSessao(Users users)
        {
            Usuariologado = users;
        }
        public static void EncerrarSessao()
        {
            Usuariologado = null;
        }
    }
}

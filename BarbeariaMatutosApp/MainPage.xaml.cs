using BarbeariaMatutosApp.Views;
namespace BarbeariaMatutosApp


{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void btnCadastro_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PushAsync(new pgUsuarioCadastro());
        }
    }

}

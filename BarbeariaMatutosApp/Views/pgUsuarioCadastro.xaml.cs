using System.Net.Http.Json;

namespace BarbeariaMatutosApp.Views;

public partial class pgUsuarioCadastro : ContentPage
{
    private readonly HttpClient httpClient;
    public pgUsuarioCadastro()
	{
		InitializeComponent();
        httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5125/")
        };
    }
    private async void btnCadastrar_Clicked(object sender, EventArgs e)
    {
        var usuario = new
        {
            Nome = txtNome.Text,
            Email = txtEmail.Text,
            Telefone = txtTelefone.Text,
            SenhaHash = txtSenha.Text
        };

        try
        {
            var response = await httpClient.PostAsJsonAsync("User/Cadastrar", usuario);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Sucesso", "Usuário cadastrado com sucesso!", "OK");
                txtNome.Text = "";
                txtEmail.Text = "";
                txtTelefone.Text = "";
                txtSenha.Text = "";
                await Shell.Current.GoToAsync("pgLoginBarbearia");
            }
            else
            {
                var erro = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Erro", $"Falha ao cadastrar: {erro}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Erro de conexão: {ex.Message}", "OK");
        }

    }
    private void TxtTelefone_TextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;

        if (entry.Text.Length > 11)
        {
            entry.Text = entry.Text.Substring(0, 11);
        }

        entry.Text = new string(entry.Text.Where(char.IsDigit).ToArray());
    }
}
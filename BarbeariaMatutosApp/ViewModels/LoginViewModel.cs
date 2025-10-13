﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http.Json;
using BarbeariaMatutosApp.Services;
using BarbeariaMatutosApp.Views;
using System.Net.Http;
using UsersDomain.Entidades;

namespace BarbeariaMatutosApp.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly ApiServices apiService;

        [ObservableProperty]
        private string? email;

        [ObservableProperty]
        private string? senha;

        // Construtor agora recebe ApiServices
        public LoginViewModel(ApiServices service)
        {
            this.apiService = service;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Senha))
            {
                await App.Current.MainPage.DisplayAlert("Atenção", "Por favor, preencha o email e a senha.", "OK");
                return;
            }

            // A LÓGICA COMPLEXA FOI SUBSTITUÍDA POR UMA ÚNICA CHAMADA DE SERVIÇO!
            var (usuario, erro) = await apiService.Login(Email, Senha);

            if (usuario !=null)
            {
                SessaoUsuarioService.IniciarSessao(usuario);

                await App.Current.MainPage.DisplayAlert("Sucesso", $"Bem-vindo, {usuario.Nome}!", "OK");

                Senha = string.Empty;
                Email = string.Empty;


                await Shell.Current.GoToAsync(nameof(pgAgendamentoServico));
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Erro", $"Motivo: {erro}", "OK");
            }
        }

        [RelayCommand]
        private async Task IrParaCadastroAsync()
        {
            await Shell.Current.GoToAsync(nameof(pgUsuarioCadastro));
        }
    }
}

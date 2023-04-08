﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DynamicMenu.EventArg;
using DynamicMenu.Models;
using DynamicMenu.Views;

namespace DynamicMenu.ViewModels
{
    public partial class AppShellViewModel : ObservableObject
    {
        private readonly AppState appState;
        public AppShellViewModel(AppState appState)
        {
            this.appState = appState;
            LoginViewModel.SuccessfulLogin += OnSuccessfullLogin;
        }

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private bool archeologieMenuButtonVisible = false;

        [ObservableProperty]
        private bool bozpMenuButtonVisible = false;

        [ObservableProperty]
        private bool monitoringMenuButtonVisible = false;

        private async void OnSuccessfullLogin(object sender, LoginEventArgs e)
        {
            var user = e.User;

            appState.CurrentUser = user;
            UserName = user.Name;
            await UpdateMenuItems(user.Rights);
        }

        [RelayCommand]
        async Task Logout()
        {
            appState.CurrentUser = null;
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
        
        private async Task GoToLogin(string message)
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            await Shell.Current.DisplayAlert("Chyba", message, "OK");
        }

        private async Task UpdateMenuItems(List<MenuButtonRight> prava)
        {
            if (prava.Count == 0)
            {
                await GoToLogin("Uživatel nemá žádná práva");
                return;
            }
            ArcheologieMenuButtonVisible = prava.Contains(MenuButtonRight.Archeologie);
            BozpMenuButtonVisible = prava.Contains(MenuButtonRight.BOZP);
            MonitoringMenuButtonVisible = prava.Contains(MenuButtonRight.Monitoring);
            GoToPage(prava.First());
        }


        private async Task GoToPage(MenuButtonRight pravo)
        {
            switch (pravo)
            {
                case MenuButtonRight.Archeologie:
                    await Shell.Current.GoToAsync($"//{nameof(ArcheologiePage)}");
                    break;
                case MenuButtonRight.BOZP:
                    await Shell.Current.GoToAsync($"//{nameof(BozpPage)}");
                    break;
                case MenuButtonRight.Monitoring:
                    await Shell.Current.GoToAsync($"//{nameof(MonitoringPage)}");
                    break;
            }
        }
    }
}

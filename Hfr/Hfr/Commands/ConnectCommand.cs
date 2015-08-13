﻿using Hfr.Helpers;
using Hfr.Model;
using Hfr.Utilities;
using Hfr.ViewModel;
using System.Diagnostics;
using System.Linq;
using System;

namespace Hfr.Commands
{
    public class ConnectCommand : Command
    {
        public override async void Execute(object parameter)
        {
            if (Loc.Main.AccountManager.Accounts.FirstOrDefault(x => x.Pseudo == Loc.Main.AccountManager.CurrentAccount.Pseudo) != null) return;
            await ThreadUI.Invoke(() =>
            {
                Loc.Main.AccountManager.CurrentAccount.ConnectionErrorStatus = "Connecting";
                Loc.Main.AccountManager.CurrentAccount.IsConnecting = true;
            });
            bool success = await Loc.Main.AccountManager.CurrentAccount.BeginAuthentication(true);
            if (success)
            {
                Loc.Main.AccountManager.Accounts.Add(Loc.Main.AccountManager.CurrentAccount);
                Loc.Main.AccountManager.AddCurrentAccountInDB();
                await ThreadUI.Invoke(() =>
                {
                    Loc.Main.AccountManager.CurrentAccount.IsConnecting = false;
                    Loc.Main.AccountManager.CurrentAccount.ConnectionErrorStatus = "Login succeeded";
                    Loc.NavigationService.Navigate(View.Main);
                });
            }
            else
            {
                await ThreadUI.Invoke(() =>
                {
                    Loc.Main.AccountManager.CurrentAccount.IsConnecting = false;
                    Loc.Main.AccountManager.CurrentAccount.ConnectionErrorStatus = "Login failed";
                });
                Debug.WriteLine("Login failed");
            }
        }
    }
}

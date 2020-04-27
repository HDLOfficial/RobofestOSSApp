﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AspNetCore.SignalR.Client;

namespace RobofestApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReadyPage : ContentPage
    {
        static HubConnection hubConnection;
        private static DateTime pageLoad;
        private static int FieldLoaded;
        public ReadyPage(int Field)
        {
            pageLoad = DateTime.Now;
            FieldLoaded = Field;
            InitializeComponent();
            SetUpSignalR();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                var existingPages = Navigation.NavigationStack.ToList();
                foreach (var page in existingPages)
                {
                    Navigation.RemovePage(page);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Navigation.PushAsync(new NotReadyPage(FieldLoaded));
            //hubConnection.StopAsync();
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            try
            {
                var existingPages = Navigation.NavigationStack.ToList();
                foreach (var page in existingPages)
                {
                    Navigation.RemovePage(page);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Navigation.PushAsync(new MainPage(FieldLoaded));
            //hubConnection.StopAsync();
        }
        async private void SetUpSignalR()
        {
            MasterServerConnection();
            await SignalRConnect();
            await SendFieldStatus();
        }
        private void MasterServerConnection()
        {
            var ip = "localhost";
            if (hubConnection == null || hubConnection.State != HubConnectionState.Connected)
            {
                hubConnection = new HubConnectionBuilder().WithUrl($"http://192.168.86.59/scoreHub").Build();
            }

            hubConnection.On<bool>("changeJudgeLock", (locked) =>
            {
                if(locked == false)
                {
                    try
                    {
                        var existingPages = Navigation.NavigationStack.ToList();
                        foreach (var page in existingPages)
                        {
                            Navigation.RemovePage(page);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    Navigation.PushAsync(new MainPage(FieldLoaded));
                    //hubConnection.StopAsync();
                }
            });

        }
        async Task SignalRConnect()
        {
            if (hubConnection == null || hubConnection.State != HubConnectionState.Connected)
            {
                Console.WriteLine("Previous Connection Terminated...");
                try
                {
                    await hubConnection.StartAsync();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                Console.WriteLine("Already Connected!");
            }
        }
        async Task SendFieldStatus()
        {
            try
            {
                await hubConnection.InvokeAsync("initField", FieldLoaded, 2, 0, "1000-1", true, false, "");
                await hubConnection.InvokeAsync("judgeClientConnection");
            }
            catch (Exception ex)
            {

            }
        }
    }
}
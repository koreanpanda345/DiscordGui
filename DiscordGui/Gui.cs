using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Terminal.Gui;

namespace DiscordGui
{
    public class Gui
    {
        private DiscordSocketClient _client;
        private CommandService _commandService;
        public Gui(DiscordSocketClient client, CommandService commandService)
        {
            _client = client;
            _commandService = commandService;
        }
        private string Logs { get; set; } = "";
        private Label LatencyLabel { get; } = new Label()
        {
            X = 0,
            Y = 0
        };
        private Label StatusLabel { get; } = new Label()
        {
            X = 0
        };
        
        private Label LogSection { get; } = new Label()
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        
        private ScrollView LogScrollView { get; } = new ScrollView()
        {
            X = 0,
            Y = Pos.Percent(50),
            Width = Dim.Fill(),
            Height = Dim.Percent(50)
        };
        public void StartApplication()
        {
            Application.Init();
            var top = Application.Top;
            
            var win = new Window("Discord Gui")
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            #region LatencyLabel
            
            LatencyLabel.Text = $"Latency: {_client.Latency.ToString()} ms";
            LatencyLabel.Width = LatencyLabel.Text.Length;

            _client.LatencyUpdated += (i, i1) =>
            {
                LatencyLabel.Text = $"Latency: {i1.ToString()} ms";
                LatencyLabel.Width = LatencyLabel.Text.Length;
                return Task.CompletedTask;
            };
            
            #endregion

            #region StatusLabel
            
            StatusLabel.Y = Pos.Bottom(LatencyLabel);
            StatusLabel.Text = $"Status: {_client.Status.ToString()}";
            StatusLabel.Width = StatusLabel.Text.Length;
            _client.CurrentUserUpdated += (user, selfUser) =>
            {
                StatusLabel.Text = $"Status: {selfUser.Status.ToString()}";
                StatusLabel.Width = StatusLabel.Text.Length;
                return Task.CompletedTask;
            };
            
            #endregion

            #region Command Labels

            var i = 1;
            _commandService.Commands.ToList().ForEach(x =>
            {
                win.Add(new Label($"{x.Name}\tDescription: {x.Summary}\tModule: {x.Module.Name}")
                {
                    X = Pos.Percent(75),
                    Y = Pos.Bottom(StatusLabel) + i,
                    Width = x.Name.Length,
                    Height = 1
                });

                i++;
            });

            #endregion
            
            #region Key Binds

            win.KeyPress += async e =>
            {
                switch (ShortcutHelper.GetModifiersKey(e.KeyEvent))
                {
                    case Key.F1: // Starts the bot.
                        await _client.StartAsync();
                        StatusLabel.Text = $"Status: {_client.Status.ToString()}";
                        StatusLabel.Width = StatusLabel.Text.Length;
                        
                        _client.Log += message =>
                        {
                            Logs += $"[{DateTime.Now}]\t({message.Source})\t{message.Message}\n";
                            LogSection.Text = Logs;
                            return Task.CompletedTask;
                        };
                        
                        #region Log Scroll View

                        LogScrollView.ContentSize = new Size(100, 50);
                        LogScrollView.ShowHorizontalScrollIndicator = true;
                        LogScrollView.ShowVerticalScrollIndicator = true;
            
                        LogScrollView.Add(LogSection);
                        win.Add(LogScrollView);
                        #endregion
                        break;
                    case Key.F2: // Stops the bot
                        if (_client.ConnectionState == ConnectionState.Connecting ||
                            _client.ConnectionState == ConnectionState.Connected)
                        {
                            await _client.StopAsync();
                            StatusLabel.Text = "Status: Offline";
                            StatusLabel.Width = StatusLabel.Text.Length;
                        }
                        break;
                    case Key.F3:
                        var online = new Button("_Online");
                        var idle = new Button("_Idle");
                        var dnd = new Button("_Do Not Disturb");
                        var invisible = new Button("_Invisible");
                        
                        var dialog = new Dialog($"Change Status from {_client.Status.ToString()} to: ", 60, 7, online, idle, dnd, invisible);
                        online.Clicked += async () =>
                        {
                            await _client.SetStatusAsync(UserStatus.Online);
                            StatusLabel.Text = "Status: Online";
                            StatusLabel.Width = StatusLabel.Text.Length;
                        };

                        idle.Clicked += async () =>
                        {
                            await _client.SetStatusAsync(UserStatus.Idle);
                            StatusLabel.Text = "Status: Idle";
                            StatusLabel.Width = StatusLabel.Text.Length;
                        };

                        dnd.Clicked += async () =>
                        {
                            await _client.SetStatusAsync(UserStatus.DoNotDisturb);
                            StatusLabel.Text = "Status: Do Not Disturb";
                            StatusLabel.Width = StatusLabel.Text.Length;
                        };

                        invisible.Clicked += async () =>
                        {
                            await _client.SetStatusAsync(UserStatus.Invisible);
                            StatusLabel.Text = "Status: Online";
                            StatusLabel.Width = StatusLabel.Text.Length;
                        };
                        
                        Application.Run(dialog);
                        break;
                    case Key.Esc: // Exits out of the program
                        Application.RequestStop();
                        break;
                }
            };

            #endregion

            #region Win Add

            win.Add(LatencyLabel);
            win.Add(StatusLabel);

            #endregion
            
            win.ColorScheme = Colors.TopLevel;
            top.Add(win);
            Application.Run(top);
        }

    }
}
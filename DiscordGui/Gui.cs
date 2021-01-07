using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.VisualBasic;
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

        private string _logs = "";

        public void StartApplication(GuiSettings settings)
        {
            Application.Init();
            var top = Application.Top;
            var win = GuiComponents.MainGui.MainWindow;

            #region LatencyLabel
            
            GuiComponents.MainGui.LatencyLabel.Text = $"Latency: {_client.Latency.ToString()} ms";
            GuiComponents.MainGui.LatencyLabel.Width = GuiComponents.MainGui.LatencyLabel.Text.Length;

            _client.LatencyUpdated += (i, i1) =>
            {
                GuiComponents.MainGui.LatencyLabel.Text = $"Latency: {_client.Latency.ToString()} ms";
                GuiComponents.MainGui.LatencyLabel.Width = GuiComponents.MainGui.LatencyLabel.Text.Length;
                return Task.CompletedTask;
            };
            
            #endregion

            #region StatusLabel
            
            GuiComponents.MainGui.StatusLabel.Y = Pos.Bottom(GuiComponents.MainGui.LatencyLabel);
            GuiComponents.MainGui.StatusLabel.Text = $"Status: {_client.Status.ToString()}";
            GuiComponents.MainGui.StatusLabel.Width = GuiComponents.MainGui.StatusLabel.Text.Length;
            _client.CurrentUserUpdated += (user, selfUser) =>
            {
                GuiComponents.MainGui.StatusLabel.Text = $"Status: {selfUser.Status.ToString()}";
                GuiComponents.MainGui.StatusLabel.Width = GuiComponents.MainGui.StatusLabel.Text.Length;
                return Task.CompletedTask;
            };
            
            #endregion

            #region ServerCountLabel
            
            GuiComponents.MainGui.ServerCountLabel.Text = $" In {_client.Guilds.Count} Servers";
            GuiComponents.MainGui.ServerCountLabel.Width = GuiComponents.MainGui.ServerCountLabel.Text.Length;
            GuiComponents.MainGui.ServerCountLabel.Height = 1;

            #endregion

            #region UsernameLabel

            GuiComponents.MainGui.UsernameLabel.Text = "Username: ";
            GuiComponents.MainGui.UsernameLabel.Height = 1;

            #endregion
            
            #region Command Labels

            var i = 0;
            _commandService.Commands.ToList().ForEach(x =>
            {
                GuiComponents.MainGui.CommandWindow.Add(new Label(x.Name)
                {
                    X = 0,
                    Y = i,
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
                        GuiComponents.MainGui.StatusLabel.Text = $"Status: {_client.Status.ToString()}";
                        GuiComponents.MainGui.StatusLabel.Width = GuiComponents.MainGui.StatusLabel.Text.Length;

                        GuiComponents.MainGui.ServerCountLabel.Text = $" In {_client.Guilds.Count} Servers";
                        GuiComponents.MainGui.ServerCountLabel.Width = GuiComponents.MainGui.ServerCountLabel.Text.Length;
                        
                        _client.Log += message =>
                        {
                            _logs += $"[{DateTime.Now}]\t({message.Source})\t{message.Message}\n";
                            GuiComponents.MainGui.LogLabel.Text = _logs;
                            GuiComponents.MainGui.UsernameLabel.Text = $"Username: {_client.CurrentUser.Username}";
                            GuiComponents.MainGui.UsernameLabel.Width = GuiComponents.MainGui.UsernameLabel.Text.Length;
                            GuiComponents.MainGui.ServerCountLabel.Text = $" In {_client.Guilds.Count} Servers";
                            GuiComponents.MainGui.ServerCountLabel.Width = GuiComponents.MainGui.ServerCountLabel.Text.Length;
                            return Task.CompletedTask;
                        };
                        
                        #region Log Scroll View
                        
                        GuiComponents.MainGui.LogScroll.ContentSize = new Size(100, 50);
                        GuiComponents.MainGui.LogScroll.ShowHorizontalScrollIndicator = true;
                        GuiComponents.MainGui.LogScroll.ShowVerticalScrollIndicator = true;
            
                        GuiComponents.MainGui.LogScroll.Add(GuiComponents.MainGui.LogLabel);
                        GuiComponents.MainGui.LogWindow.Add(GuiComponents.MainGui.LogScroll);
                        win.Add(GuiComponents.MainGui.LogWindow);
                        #endregion
                        break;
                    case Key.F2: // Stops the bot
                        if (_client.ConnectionState == ConnectionState.Connecting ||
                            _client.ConnectionState == ConnectionState.Connected)
                        {
                            await _client.StopAsync();
                            GuiComponents.MainGui.StatusLabel.Text = "Status: Offline";
                            GuiComponents.MainGui.StatusLabel.Width = GuiComponents.MainGui.StatusLabel.Text.Length;
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
                            GuiComponents.MainGui.StatusLabel.Text = "Status: Online";
                            GuiComponents.MainGui.StatusLabel.Width = GuiComponents.MainGui.StatusLabel.Text.Length;
                            Application.RequestStop();
                        };

                        idle.Clicked += async () =>
                        {
                            await _client.SetStatusAsync(UserStatus.Idle);
                            GuiComponents.MainGui.StatusLabel.Text = "Status: Idle";
                            GuiComponents.MainGui.StatusLabel.Width = GuiComponents.MainGui.StatusLabel.Text.Length;
                            Application.RequestStop();

                        };

                        dnd.Clicked += async () =>
                        {
                            await _client.SetStatusAsync(UserStatus.DoNotDisturb);
                            GuiComponents.MainGui.StatusLabel.Text = "Status: Do Not Disturb";
                            GuiComponents.MainGui.StatusLabel.Width = GuiComponents.MainGui.StatusLabel.Text.Length;
                            Application.RequestStop();
                        };

                        invisible.Clicked += async () =>
                        {
                            await _client.SetStatusAsync(UserStatus.Invisible);
                            GuiComponents.MainGui.StatusLabel.Text = "Status: Online";
                            GuiComponents.MainGui.StatusLabel.Width = GuiComponents.MainGui.StatusLabel.Text.Length;
                            Application.RequestStop();
                        };
                        
                        Application.Run(dialog);
                        break;
                    case Key.Esc: // Exits out of the program
                        Application.RequestStop();
                        break;
                }
            };

            #endregion

            #region Menu

            var menu = new MenuBar(new[]
            {
                new MenuBarItem("_Application", new[]
                {
                    new MenuItem("_Quit", "Exit out of the program.", () =>
                    {
                        Application.RequestStop();
                    })
                })
            });
            
            #endregion

            #region Win Add
            if(!settings.DisableTips)
                GuiComponents.MainGui.InformationWindow.Add(GuiComponents.MainGui.TooltipsLabel);
            win.Add(GuiComponents.MainGui.CommandWindow);
            GuiComponents.MainGui.StatsWindow.Add(GuiComponents.MainGui.ServerCountLabel);
            GuiComponents.MainGui.StatsWindow.Add(GuiComponents.MainGui.LatencyLabel);
            GuiComponents.MainGui.StatsWindow.Add(GuiComponents.MainGui.StatusLabel);
            GuiComponents.MainGui.StatsWindow.Add(GuiComponents.MainGui.UsernameLabel);
            win.Add(GuiComponents.MainGui.StatsWindow);
            win.Add(GuiComponents.MainGui.InformationWindow);
            #endregion
            
            win.ColorScheme = Colors.TopLevel;
            top.Add(win, menu);
            Application.Run(top);
        }
    }
}
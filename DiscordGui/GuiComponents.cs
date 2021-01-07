using Terminal.Gui;

namespace DiscordGui
{
    internal static class GuiComponents
    {
        public static class MainGui
        {

            #region Buttons

            public static class StatusDialogButtonGroup
            {
                public static Button Online { get; set; } = new Button("_Online");
                public static Button Idle { get; set; } = new Button("_Idle");
                public static Button DoNotDisturb { get; set; } = new Button("_Do Not Disturb");
                public static Button Invisible { get; set; } = new Button("_Invisible");
            }

            #endregion

            #region Dialogs

            public static Dialog StatusChange = new Dialog(
                "Change Status To:",
                60, 
                7, 
                StatusDialogButtonGroup.Online,
                StatusDialogButtonGroup.Idle,
                StatusDialogButtonGroup. DoNotDisturb,
                StatusDialogButtonGroup.Invisible
                );

            #endregion
            
            #region Labels
            public static Label LatencyLabel { get; set; } = new Label()
            {
                X = 1,
                Y = 0
            };
            public static Label StatusLabel { get; set; } = new Label()
            {
                X = 1,
                Y = Pos.Bottom(LatencyLabel)
            };
            public static Label ServerCountLabel { get; set; } = new Label()
            {
                X = Pos.Right(StatusLabel) + 4,
                Y = Pos.Y(StatusLabel)
            };
            
            public static Label UsernameLabel { get; set; } = new Label()
            {
                X = Pos.Right(LatencyLabel) + 4,
                Y = Pos.Y(LatencyLabel)
            };
            
            public static Label LogLabel { get; set; } = new Label()
            {
                X = 4,
                Y = 0,
                Width = Dim.Fill() -4,
                Height = Dim.Fill()
            };

            public static Label TooltipsLabel { get; set; } = new Label("F1 - Start Bot | F2 - Stop Bot | F3 - Change Status")
            {
                X = Pos.Center(),
                Y = 0,
                Width = "F1 - Start Bot | F2 - Stop Bot | F3 - Change Status".Length,
                Height = 1
            };
            
            #endregion

            #region Scrolls
            
            public static ScrollView LogScroll { get; set; } = new ScrollView()
            {
                X = 4,
                Y = 0,
                Width = Dim.Fill() - 4,
                Height = Dim.Fill()
            };
            public static ScrollView MessageScroll { get; set; } = new ScrollView()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            #endregion

            #region Windows
            public static Window CommandWindow { get; set; } = new Window("Commands List")
            {
                X = Pos.Percent(75),
                Y = 0,
                Width = Dim.Percent(25),
                Height = Dim.Percent(25),
                ColorScheme = Colors.TopLevel
            };
            
            public static Window InformationWindow { get; set; } = new Window("Information")
            {
                X = Pos.Center(),
                Y = 0,
                Width = TooltipsLabel.Width + 10,
                Height = Dim.Percent(10),
                ColorScheme = Colors.TopLevel
            };
            public static Window MainWindow { get; set; } = new Window("Discord GUI")
            {
                X = 0,
                Y = 0,
                Width =  Dim.Fill(),
                Height = Dim.Fill(),
                ColorScheme = Colors.TopLevel
            };
            
            public static Window LogWindow { get; set; } = new Window("Logs")
            {
                X = 4,
                Y = Pos.Percent(50),
                Width = Dim.Fill() - 4,
                Height = Dim.Percent(50),
                ColorScheme = Colors.TopLevel
            };
            
            public static Window StatsWindow { get; set; } = new Window("Bot's Stats & Information")
            {
                X = 1,
                Y = 0,
                Width = Dim.Percent(25),
                Height = Dim.Percent(25),
                ColorScheme = Colors.TopLevel
            };

            #endregion
        }

    }
}
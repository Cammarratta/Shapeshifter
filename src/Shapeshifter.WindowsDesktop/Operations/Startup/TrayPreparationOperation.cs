﻿namespace Shapeshifter.WindowsDesktop.Operations.Startup
{
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using Controls.Window.Factories.Interfaces;

    using Infrastructure.Events;

    using Interfaces;

    using Services.Interfaces;

    using Application = System.Windows.Application;

    public class TrayPreparationOperation: ITrayPreparationOperation
    {
        readonly ITrayIconManager trayIconManager;
        readonly ISettingsWindowFactory settingsWindowFactory;

        public TrayPreparationOperation(
            ITrayIconManager trayIconManager,
            ISettingsWindowFactory settingsWindowFactory)
        {
            this.trayIconManager = trayIconManager;
            this.settingsWindowFactory = settingsWindowFactory;
        }

        public async Task RunAsync()
        {
            trayIconManager.IconClicked += TrayIconManager_IconClicked;
            trayIconManager.InstallTrayIcon(
                "Settings",
                new[]
                {
                    new MenuItem(
                    "Exit",
                    (sender, e) => Application.Current.Shutdown())
                });
        }

        void TrayIconManager_IconClicked(object sender, TrayIconClickedEventArgument e)
        {
            var window = settingsWindowFactory.Create();
            window.Show();
        }
    }
}
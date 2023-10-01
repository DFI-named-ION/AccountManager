using System.Windows;
using AccountManager.MVVM.Views.Windows;

namespace AccountManager
{
    public partial class App : Application
    {
        protected void ApplicationStart(object sender, StartupEventArgs args)
        {
            var authWindow = new AuthWindow();
            var programWindow = new ProgramWindow();

            authWindow.Show();
            
            programWindow.IsVisibleChanged += (s, e) =>
            {
                if (programWindow.Visibility == Visibility.Hidden && programWindow.IsLoaded)
                {
                    authWindow.Show();
                }
            };
            programWindow.Closed += (s, e) =>
            {
                Application.Current.Shutdown();
            };

            authWindow.IsVisibleChanged += (s, e) =>
            {
                if (authWindow.Visibility == Visibility.Hidden && authWindow.IsLoaded)
                {
                    programWindow.Show();
                }
            };
            authWindow.Closed += (s, e) =>
            {
                Application.Current.Shutdown();
            };
        }
    }
}
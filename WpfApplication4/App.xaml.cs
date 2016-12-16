using System.Windows;

namespace ScopeViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static MainWindow mainWindow;

        private void App_Startup(object sender, StartupEventArgs e)
        {

            mainWindow = new MainWindow();
            mainWindow.Show();
            if (e.Args.Length > 0)
            {
                mainWindow.OpenAuto(e.Args[0]);
            }
        }
    }
}

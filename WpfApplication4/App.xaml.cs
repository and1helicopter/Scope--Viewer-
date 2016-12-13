using System.Windows;

namespace ScopeViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            if (e.Args.Length > 0)
            {
                mainWindow.OpenAuto(e.Args[0]);
            }
        }
    }
}

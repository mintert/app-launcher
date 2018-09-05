using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using Microsoft.Win32;

namespace AppLauncher
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string URI_SCHEME = "verstehe-ra";
        const string URI_KEY = "URL:Verstehe Recording Application";
        string[] args;

        public MainWindow()
        {
            args = Environment.GetCommandLineArgs();
            InitializeComponent();

            if (args.Length == 1)
            {
                Prepare();
                Application.Current.Shutdown();
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            Load();

            foreach(string argument in args)
            {
                textBox.Text += argument + "\n";
            }
        }

        private void Prepare()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.WebComputing";
            string fileLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string fileTarget = path + "\\AppLauncher.exe";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (File.Exists(fileTarget))
            {
                File.Delete(fileTarget);
            }

            File.Move(fileLocation, fileTarget);

            RegisterProtocolHandler(URI_SCHEME, URI_KEY, fileTarget);
        }

        private void RegisterProtocolHandler(string scheme, string key, string app)
        {
            string cmdValue = String.Format("\"{0}\" \"%1\"", app);
            string iconValue = String.Format("\"{0}\",0", app);

            RegistryKey keyGroup = Registry.CurrentUser.OpenSubKey("Software\\Classes", true);
            RegistryKey regKey = keyGroup.CreateSubKey(scheme);
            regKey.SetValue("", key);
            regKey.SetValue("URL Protocol", "");
            regKey.CreateSubKey("DefaultIcon").SetValue("", iconValue);
            regKey.CreateSubKey("shell\\open\\command").SetValue("", cmdValue);
        }

        private void UnregisterProtocolHandler(string scheme)
        {
            RegistryKey keyGroup = Registry.CurrentUser.OpenSubKey("Software\\Classes", true);
            keyGroup.DeleteSubKeyTree(scheme);
        }

        async private void Load()
        {
            for (int i = 0; i <= 100; i++)
            {
                await Task.Delay(50);
                Progress.Value = i;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            UnregisterProtocolHandler(URI_SCHEME);
            Thread.Sleep(50);
            Application.Current.Shutdown();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

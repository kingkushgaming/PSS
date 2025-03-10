using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;
using System.Diagnostics;
using Project.services;

namespace Project
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();


            DB db = DB.Instance;

            if (db.TableExists("User"))
            {


                Login login = new(db);

                this.Hide();
                this.ShowInTaskbar = false;

                var s = login.Authentificate();

                if (s)
                {
                    this.Show();
                    this.ShowInTaskbar = true;
                }
            }
            else
            {
                Setup setup = new(db);

                this.Hide();
                this.ShowInTaskbar = false;

                var s = setup.setUp();

                if (s)
                {
                    this.Show();
                    this.ShowInTaskbar = true;
                }
            }
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            this.IconX.Visibility = Visibility.Visible;
            this.IconMinus.Visibility = Visibility.Visible;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            this.IconX.Visibility = Visibility.Hidden;
            this.IconMinus.Visibility = Visibility.Hidden;
        }


        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Minimize_Click(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AppDataSave_service appData = new();
            appData.DeleteUserLoginData();

            Process.Start(Process.GetCurrentProcess().MainModule.FileName);

            Environment.Exit(0);
        }

    }
}
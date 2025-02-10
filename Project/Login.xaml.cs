using Project.objects;
using Project.services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xaml;

namespace Project
{
    /// <summary>
    /// Interaktionslogik für Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        // Result of the login
        bool result = false;

        // Service instans of Appdata
        AppDataSave_service appData = new();

        // Database instance
        DB db;

        // UserLoginData instance
        UserDataLogin udl;


        /// <summary>
        /// the Login Window when created needs db connection
        /// </summary>
        /// <param name="db">database connection</param>
        public Login(DB db)
        {
            this.db = db;
            udl = new(db.Get("SELECT*FROM User")[1], db.Get("SELECT*FROM User")[2], false);
            InitializeComponent();
            this.ShowInTaskbar = true;
            this.EmailTbx.Focus();
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            this.IconX.Visibility = Visibility.Visible;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            this.IconX.Visibility = Visibility.Hidden;
        }

        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Authentificates the User when it has the correct data saved in UserData
        /// </summary>
        /// <returns>When the use is Autheticatet return true else open the window to log the user in and if correct the true else false</returns>
        public bool Authentificate()
        {
            UserDataLogin obj = appData.LoadUserLoginData();

            if (obj != null)
            {
                if (obj.Email == udl.Email && obj.PasswordHash == udl.PasswordHash)
                {
                    return true;
                }
            }

            this.ShowDialog();
            return result;
        }



        private void Border_KeyDown(object sender, MouseEventArgs e)
        {
            setLoading(true);
            LoginUser();
        }

        private void setLoading(bool isloading)
        {

            if (isloading)
            {
                this.LoadingImg.Visibility = Visibility.Visible;
                this.LoginBtnName.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.LoginBtnName.Visibility = Visibility.Visible;
                this.LoadingImg.Visibility = Visibility.Collapsed;
            }
        }

        private void LoginBtnName_KeyDown(object sender, MouseEventArgs e)
        {
            Border_KeyDown(sender, e);
        }

        private async void LoginUser()
        {
            this.EmailErrorLbl.Visibility = Visibility.Hidden;
            this.PasswordErrorLbl.Visibility = Visibility.Hidden;
            setLoading(true);

            string email = this.EmailTbx.Text.Trim().ToLower();
            string password = this.PasswordTbx.Password;

            await Task.Delay(100);

            if (email == udl.Email && udl.ValidatePassword(password))
            {

                await Task.Delay(1000);

                setLoading(false);

                result = true;

                if (RememberMeCheckBox.IsChecked == true)
                {
                    appData.SaveUserLoginData(new { Email = udl.Email, Password = udl.PasswordHash });
                }

                this.Close();
                return;
            }


            await Task.Delay(500);

            this.EmailTbx.Text = "";
            this.EmailErrorLbl.Visibility = Visibility.Visible;
            this.PasswordTbx.Password = "";
            this.PasswordErrorLbl.Visibility = Visibility.Visible;


            setLoading(false);
        }

        private void PasswordTbx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Border_KeyDown(sender, null);
            }
        }
    }
}

using Project.objects;
using Project.services;
using System.Diagnostics;
using System.DirectoryServices;
using System.Windows;
using System.Windows.Input;

namespace Project;

public partial class Setup : Window
{
    private DB db;
    public Setup(DB db)
    {
        this.db = db;
        InitializeComponent();
    }

    private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        this.PasswrodsError.Visibility = Visibility.Hidden;



        if (this.PasswordTbx.Password != this.ConfirmPasword.Password)
        {
            this.PasswrodsError.Visibility = Visibility.Visible;
            return;
        }

        this.SetupScreen.Visibility = Visibility.Collapsed;
        this.LoadingScreen.Visibility = Visibility.Visible;

        CreateAccount();
    }

    public bool setUp()
    {
        this.ShowDialog();

        if (this.DialogResult == true) { return true; }
        return false;
    }

    private void CreateAccount()
    {
        db.Execute(@"
            CREATE TABLE IF NOT EXISTS User (
                id INTEGER PRIMARY KEY CHECK (id = 1), 
                email TEXT NOT NULL,
                password TEXT NOT NULL
            );
        ");

        db.Execute("INSERT INTO User(id, email, password) VALUES ('1', ?, ?)", this.EmailTbx.Text, UserDataLogin.HashPassword(this.PasswordTbx.Password));

        string[] s = db.Get("SELECT * FROM User");

        foreach (string s2 in s) 
        {
            Debug.WriteLine(s2);
        }

        this.DialogResult = true;
        this.Close();
    }

    private void Close_Click(object sender, MouseButtonEventArgs e)
    {
        Application.Current.Shutdown();   
    }
}
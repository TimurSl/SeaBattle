using System.Windows;
using SeaBattle.Account.Providers;

namespace SeaBattleLauncher;

public partial class AuthWindow : Window
{
	public Account account;
	private IAccountProvider AccountProvider = new WebAccountProvider();
	public AuthWindow()
	{
		InitializeComponent ();
		
		AuthButton.Click += Authorize;
	}
	
	public AuthWindow(IAccountProvider accountProvider) : this()
	{
		AccountProvider = accountProvider;
	}

	private void Authorize(object sender, RoutedEventArgs e)
	{
		if (string.IsNullOrWhiteSpace(LoginInput.Text) || string.IsNullOrWhiteSpace(PasswordInput.Password))
		{
			MessageBox.Show("Login or password is empty");
			return;
		}
		
		// make parameters login and password (paramerized thread) and get account
		
		ParameterizedThreadStart threadStart = new ParameterizedThreadStart((obj) =>
		{
			var (login, password) = ((string, string))obj;
			account = AccountProvider.GetAccount(login, password);
			Console.WriteLine(account.Login);
		});
		Thread thread = new Thread(threadStart);
		thread.Start((LoginInput.Text, PasswordInput.Password));

		// close window
		var msg = MessageBox.Show("Authorization successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
		if (msg == MessageBoxResult.OK)
		{
			Close ();
		}
	}
}
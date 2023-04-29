using System.Windows;
using System.Windows.Controls;
using SeaBattle.Account.Providers;
using SeaBattle.Settings;

namespace SeaBattleLauncher;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private int playerCount = 0;
    private PlayerType playerType = PlayerType.Bot;
    private AccountType accountType = AccountType.Guest;
    private BotDifficulties botDifficulty = BotDifficulties.Honest;
    private string playerName = "";
    
    public Account Account { get; private set; }

    public MainWindow()
	{
		InitializeComponent ();
        
        PlayerNameInput.Text = "Enter name";
        PlayerNameInput.TextChanged += (sender, args) => playerName = PlayerNameInput.Text;
        
        PlayerTypeComboBox.SelectionChanged += PlayerTypeChanged;
        BotDifficultyComboBox.SelectionChanged += BotDifficultyChanged;
        AccountTypeComboBox.SelectionChanged += AccountTypeChanged;
        
        AuthorizeButton.Click += Authorize;
    }

    private void Authorize(object sender, RoutedEventArgs e)
    {
        // open AuthWindow, then get data from it (Account)
        IAccountProvider accountProvider = accountType == AccountType.Guest ? new GuestProvider() : new WebAccountProvider();
        AuthWindow authWindow = new AuthWindow(accountProvider);
        authWindow.ShowDialog();

        authWindow.Closed += (o, args) =>
        {
            Account = authWindow.account;
            authWindow.Close();
        };
    }

    private void AccountTypeChanged(object sender, SelectionChangedEventArgs e)
    {
        accountType = (AccountType)AccountTypeComboBox.SelectedIndex;
        AuthorizeButton.Visibility = accountType == AccountType.Account ? Visibility.Visible : Visibility.Collapsed;
    }

    private void BotDifficultyChanged(object sender, SelectionChangedEventArgs e)
    {
        botDifficulty = (BotDifficulties)BotDifficultyComboBox.SelectedIndex;
    }

    private void PlayerTypeChanged(object sender, SelectionChangedEventArgs e)
    {
        playerType = (PlayerType)PlayerTypeComboBox.SelectedIndex;
        BotDifficultyComboBox.Visibility = playerType == PlayerType.Bot ? Visibility.Visible : Visibility.Collapsed;
        AccountTypeComboBox.Visibility = playerType == PlayerType.Player ? Visibility.Visible : Visibility.Collapsed;
    }

    public void RemoveText(object sender, EventArgs e)
    {
        if (PlayerNameInput.Text == "Enter name")
        {
            PlayerNameInput.Text = "";
        }
    }

    public void AddText(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(PlayerNameInput.Text))
            PlayerNameInput.Text = "Enter name";
    }

}

public enum PlayerType
{
    Bot = 0,
    Player = 1,
}

public enum AccountType
{
    Guest = 0,
    Account = 1,
}
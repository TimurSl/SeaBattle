using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using SeaBattle.Account.Providers;
using SeaBattle.Core;
using SeaBattle.Core.Types;
using SeaBattle.Players;
using SeaBattle.Players.Inputs;
using SeaBattle.Players.Inputs.Bot;
using SeaBattle.Players.Interfaces;
using SeaBattle.Players.Types;
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
    
    private List<Player> players = new();

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
        NextPlayerButton.Click += NextPlayer;
    }

    private void NextPlayer(object sender, RoutedEventArgs e)
    {
        PlayerParams playerParams = new();
        playerParams.Name = playerName;
        playerParams.Type = playerType;
        
        if (playerType == PlayerType.Bot)
        {
            
            switch (botDifficulty)
            {
                case BotDifficulties.PatrickStar:
                    playerParams.Input = new EasyBotInput();
                    break;
                case BotDifficulties.Honest:
                    playerParams.Input = new MediumBotInput();
                    break;
                case BotDifficulties.Hard:
                    playerParams.Input = new HardBotInput();
                    break;
            }
        }
        else
        {
            playerParams.Input = new PlayerArrowInput();
        }
        
        playerParams.Account = Account;


        Player player = new Player(playerParams);
        players.Add(player);

        // set next player text
        PlayerCount.Content = $"Player {players.Count + 1}";

        if (players.Count == 2)
        {
            GameLaunchParams gameLaunchParams = new();
            gameLaunchParams.Players = players;
            Game game = new Game(gameLaunchParams);
            game.Start();
        }
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
        };

        if (Account != null)
        {
            ViewStatsButton.Visibility = Visibility.Visible;
            ViewStatsButton.Click += (o, args) =>
            {
                MessageBox.Show($"Wins: {Account.Stats.Wins}", $"Account stats: {Account.Login}");
            };
        }
    }

    private void AccountTypeChanged(object sender, SelectionChangedEventArgs e)
    {
        accountType = (AccountType)AccountTypeComboBox.SelectedIndex;
        AuthorizeButton.Visibility = accountType == AccountType.Account ? Visibility.Visible : Visibility.Collapsed;
        ViewStatsButton.Visibility = accountType == AccountType.Account ? Visibility.Visible : Visibility.Collapsed;
    }

    private void BotDifficultyChanged(object sender, SelectionChangedEventArgs e)
    {
        botDifficulty = (BotDifficulties)BotDifficultyComboBox.SelectedIndex;
    }

    private void PlayerTypeChanged(object sender, SelectionChangedEventArgs e)
    {
        playerType = (PlayerType)PlayerTypeComboBox.SelectedIndex + 1;
        Console.WriteLine(playerType);
        BotDifficultyComboBox.Visibility = playerType == PlayerType.Bot ? Visibility.Visible : Visibility.Collapsed;
        AccountTypeComboBox.Visibility = playerType == PlayerType.Human ? Visibility.Visible : Visibility.Collapsed;
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

public enum AccountType
{
    Guest = 0,
    Account = 1,
}
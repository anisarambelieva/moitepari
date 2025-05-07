using MySqlConnector;

namespace MoitePari
{
    public partial class MainPage : ContentPage
    {
        readonly MySqlConnection _db;

        public MainPage(MySqlConnection db)
        {
            InitializeComponent();
            _db = db;
        }

        private async void OnAddDepositClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync(nameof(AddDepositPage));

        private async void OnViewDepositsClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync(nameof(DepositListPage));

    }
}

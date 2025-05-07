using MoitePari.Models;
using MySqlConnector;
using System.Data;

namespace MoitePari
{
    public partial class DepositListPage : ContentPage
    {
        readonly MySqlConnection _db;

        public DepositListPage(MySqlConnection db)
        {
            InitializeComponent();
            _db = db;
            _ = LoadDepositsAsync();
        }

        async Task LoadDepositsAsync()
        {
            var list = new List<Deposit>();

            try
            {
                using var cmd = new MySqlCommand(
                    "SELECT id, name, min_amount, max_amount, interest_rate, term_months, currency, fee_type, payment_frequency, notes FROM deposits",
                    _db);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new Deposit
                    {
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("name"),
                        MinAmount = reader.GetDecimal("min_amount"),
                        MaxAmount = reader.GetDecimal("max_amount"),
                        InterestRate = reader.GetDecimal("interest_rate"),
                        TermMonths = reader.GetInt32("term_months"),
                        Currency = reader.GetString("currency"),
                        FeeType = reader.IsDBNull("fee_type") ? null : reader.GetString("fee_type"),
                        PaymentFrequency = reader.IsDBNull("payment_frequency") ? null : reader.GetString("payment_frequency"),
                        Notes = reader.IsDBNull("notes") ? null : reader.GetString("notes"),
                    });
                }

                DepositsCollection.ItemsSource = list;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error loading deposits", ex.Message, "OK");
            }
        }
    }
}

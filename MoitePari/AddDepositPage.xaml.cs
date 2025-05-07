using MySqlConnector;

namespace MoitePari
{
  public partial class AddDepositPage : ContentPage
  {
    readonly MySqlConnection _db;
    public AddDepositPage(MySqlConnection db)
    {
      InitializeComponent();
      _db = db;
    }

    async void OnSaveClicked(object sender, EventArgs e)
    {
      try
      {
        var name = NameEntry.Text.Trim();
        var min  = decimal.Parse(MinAmountEntry.Text);
        var max  = decimal.Parse(MaxAmountEntry.Text);
        var rate = decimal.Parse(InterestRateEntry.Text);
        var term = int.Parse(TermMonthsEntry.Text);
        var fee  = FeeTypeEntry.Text?.Trim();
        var freq = PaymentFreqEntry.Text?.Trim();
        var notes= NotesEditor.Text?.Trim();

        if (min < 0 || max < min) throw new Exception("Invalid amounts");

        const string sql = @"
          INSERT INTO deposits
            (name, min_amount, max_amount, interest_rate, term_months,
             currency, fee_type, payment_frequency, notes)
          VALUES
            (@name,@min,@max,@rate,@term,'BGN',@fee,@freq,@notes);";

        using var cmd = new MySqlCommand(sql, _db);
        cmd.Parameters.AddWithValue("@name", name);
        cmd.Parameters.AddWithValue("@min", min);
        cmd.Parameters.AddWithValue("@max", max);
        cmd.Parameters.AddWithValue("@rate", rate);
        cmd.Parameters.AddWithValue("@term", term);
        cmd.Parameters.AddWithValue("@fee", fee);
        cmd.Parameters.AddWithValue("@freq", freq);
        cmd.Parameters.AddWithValue("@notes", notes);

        var rows = await cmd.ExecuteNonQueryAsync();
        await DisplayAlert("Success", $"Inserted {rows} deposit.", "OK");
        await Shell.Current.GoToAsync("..");
      }
      catch (Exception ex)
      {
        await DisplayAlert("Error", ex.Message, "OK");
      }
    }
  }
}

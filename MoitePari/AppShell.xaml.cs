namespace MoitePari;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(AddDepositPage), typeof(AddDepositPage));
		Routing.RegisterRoute(nameof(DepositListPage), typeof(DepositListPage));
	}
}

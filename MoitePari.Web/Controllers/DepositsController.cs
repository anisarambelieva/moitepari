using Microsoft.AspNetCore.Mvc;
using MoitePari.BusinessLogic;
using MoitePari.BusinessLogic.Models;
using MySqlConnector;

namespace MoitePari.Web.Controllers
{
    /// <summary>
    /// Handles HTTP requests related to bank deposit products, including listing,
    /// retrieving, and calculating repayment plans.
    /// </summary>
    public class DepositsController : Controller
    {
        private readonly MySqlConnection _db;
        private readonly DepositCalculator _calc;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepositsController"/> class
        /// with a database connection and a calculator for deposit computations.
        /// </summary>
        /// <param name="db">The active MySQL database connection.</param>
        /// <param name="calc">An instance of <see cref="DepositCalculator"/> used to compute plans.</param>
        public DepositsController(MySqlConnection db, DepositCalculator calc)
        {
            _db = db;
            _calc = calc;
        }

        /// <summary>
        /// Displays a list of all available deposit products.
        /// </summary>
        /// <returns>An HTML view with a list of <see cref="DepositModel"/> items.</returns>
        public async Task<IActionResult> Index()
        {
            var list = new List<DepositModel>();
            using var cmd = new MySqlCommand(
                @"SELECT 
                    id,
                    name,
                    min_amount,
                    max_amount,
                    interest_rate,
                    term_months,
                    fee_type
                  FROM deposits",
                _db);

            using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                // Safe read of fee_type as string
                string feeTypeRaw = rdr.IsDBNull(rdr.GetOrdinal("fee_type"))
                    ? "0%"
                    : rdr.GetString(rdr.GetOrdinal("fee_type"));

                // Trim trailing '%' and parse to decimal, with a fallback
                if (!decimal.TryParse(
                        feeTypeRaw.TrimEnd('%'),
                        out var feePct))
                {
                    feePct = 0m;
                }

                list.Add(new DepositModel
                {
                    Id = rdr.GetInt32("id"),
                    Name = rdr.GetString("name"),
                    MinAmount = rdr.GetDecimal("min_amount"),
                    MaxAmount = rdr.GetDecimal("max_amount"),
                    InterestRate = rdr.GetDecimal("interest_rate"),
                    TermMonths = rdr.GetInt32("term_months"),
                    FeePercentage = feePct
                });
            }

            return View(list);
        }

        /// <summary>
        /// Displays a form for calculating a repayment plan for the selected deposit product.
        /// </summary>
        /// <param name="id">The ID of the deposit product to calculate.</param>
        /// <returns>A view with pre-filled product data or NotFound if not found.</returns>
        public async Task<IActionResult> Calculate(int id)
        {
            using var cmd = new MySqlCommand(
                @"SELECT id, name, min_amount, max_amount, interest_rate, term_months, fee_type
                  FROM deposits
                  WHERE id = @id",
                _db);
            cmd.Parameters.AddWithValue("@id", id);

            using var rdr = await cmd.ExecuteReaderAsync();
            if (!await rdr.ReadAsync())
                return NotFound();

            int feeTypeOrd = rdr.GetOrdinal("fee_type");
            string feeRaw = rdr.IsDBNull(feeTypeOrd)
                ? "0%"
                : rdr.GetString(feeTypeOrd);

            if (!decimal.TryParse(feeRaw.TrimEnd('%'), out var feePct))
                feePct = 0m;

            var product = new DepositModel
            {
                Id = id,
                Name = rdr.GetString(rdr.GetOrdinal("name")),
                MinAmount = rdr.GetDecimal(rdr.GetOrdinal("min_amount")),
                MaxAmount = rdr.GetDecimal(rdr.GetOrdinal("max_amount")),
                InterestRate = rdr.GetDecimal(rdr.GetOrdinal("interest_rate")),
                TermMonths = rdr.GetInt32(rdr.GetOrdinal("term_months")),
                FeePercentage = feePct
            };

            return View(product);
        }

        /// <summary>
        /// Processes the user's input and generates a repayment plan based on the
        /// selected deposit and input amount/term.
        /// </summary>
        /// <param name="product">The deposit product selected by the user.</param>
        /// <param name="amount">The deposit amount entered by the user.</param>
        /// <param name="termMonths">The term in months entered by the user.</param>
        /// <returns>A view displaying the calculated repayment plan.</returns>
        [HttpPost]
        public IActionResult Calculate(
            [FromForm] DepositModel product,
            [FromForm] decimal amount,
            [FromForm] int termMonths)
        {
            var plan = _calc.Calculate(product, amount, termMonths);
            return View("Plan", plan);
        }
    }
}

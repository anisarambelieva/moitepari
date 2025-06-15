using Microsoft.AspNetCore.Mvc;
using MoitePari.BusinessLogic;
using MoitePari.BusinessLogic.Models;
using MySqlConnector;

namespace MoitePari.Web.Controllers
{
    public class DepositsController : Controller
    {
        readonly MySqlConnection _db;
        readonly DepositCalculator _calc;

        public DepositsController(MySqlConnection db, DepositCalculator calc)
        {
            _db = db;
            _calc = calc;
        }

        // GET /Deposits
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

        // GET /Deposits/Calculate/{id}
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

        // POST /Deposits/Calculate
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionsAPI.Models;

namespace TransactionsAPITests.DataHelper
{
    internal class TestDataHelper
    {
        public static List<Transaction> GetFakeTransactionsList()
        {
            return new List<Transaction>()
            {
                new Transaction
                {
                    Id = new Guid("ff289918-5ab7-4133-8f75-ab6cc508497a"),
                    ApplicationName = "Viva",
                    Email = "lcubbit0@walmart.com",
                    Filename = "Nonummy.png",
                    Url = new Uri("http://vkontakte.ru"),
                    Inception = new DateTime(2017, 8, 2),
                    Amount = (decimal) 661.65,
                    AmountCurrency = '$',
                    Allocation = (decimal) 26.02
                },
                new Transaction
                {
                    Id = new Guid("06544e34-061b-4eec-b4fc-d14c489dd648"),
                    ApplicationName = "Greenlam",
                    Email = "arickert1@wordpress.org",
                    Filename = "DuisMattisEgestas.tiff",
                    Url = new Uri("http://twitpic.com"),
                    Inception = new DateTime(2021, 11, 22),
                    Amount = (decimal) 818.18,
                    AmountCurrency = '$',
                    Allocation = (decimal) 26.07
                }
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuntaNoastra_Buta_Camelia.Models;

namespace NuntaNoastra_Buta_Camelia.Data
{
    public class DbInitializer
    {
        public static void Initialize(ShopContext context)
        {
            context.Database.EnsureCreated();
            if (context.Candles.Any())
            {
                return; // BD a fost creata anterior
            }
            var candles = new Candle[]
            {
                 new Candle{Name="Fericire",Distribuitor="Sucevita",Price=Decimal.Parse("22")},
                 new Candle{Name="Căsnicie",Distribuitor="Casa cu fericire",Price=Decimal.Parse("18")},
                 new Candle{Name="Iubire",Distribuitor="Nunta de vis",Price=Decimal.Parse("27")}
            };
            foreach (Candle l in candles)
            {
                context.Candles.Add(l);
            }
            context.SaveChanges();
            var customers = new Customer[]
            {

                 new Customer{CustomerID=1050,Name="Popescu Marcela",BirthDate=DateTime.Parse("1979-09-01")},
                 new Customer{CustomerID=1045,Name="Mihailescu Cornel",BirthDate=DateTime.Parse("1969-07-08")},

            };
            foreach (Customer c in customers)
            {
                context.Customers.Add(c);
            }
            context.SaveChanges();
            var orders = new Order[]
            {
                 new Order{CandleID=1,CustomerID=1050,OrderDate=DateTime.Parse("02-25-2020")},
                 new Order{CandleID=3,CustomerID=1045,OrderDate=DateTime.Parse("02-25-2020")},
                 new Order{CandleID=1,CustomerID=1045,OrderDate=DateTime.Parse("02-25-2020")},
                 new Order{CandleID=2,CustomerID=1050,OrderDate=DateTime.Parse("02-25-2020")},
            };
            foreach (Order e in orders)
            {
                context.Orders.Add(e);
            }
            context.SaveChanges();

            var distribuitors = new Distribuitor[]
            {

                new Distribuitor {DistribuitorName = "Sucevița", Adress = "Str. Aviatorilor, nr. 40, Cluj-Napoca"},
                new Distribuitor {DistribuitorName = "Casa cu fericire", Adress = "Str. Horea, nr. 83, Cluj-Napoca"},
                new Distribuitor {DistribuitorName = "Nunta de vis", Adress = "Str. Dorea, nr. 7, Cluj-Napoca"},
            };
            foreach (Distribuitor p in distribuitors)
            {
                context.Distribuitors.Add(p);
            }
            context.SaveChanges();
            var distribuitorcandles = new DistribuitorCandle[]
            {
                     new DistribuitorCandle { CandleID = candles.Single(c => c.Name == "Fericire" ).ID,DistribuitorID = distribuitors.Single(i => i.DistribuitorName =="Sucevița").ID},
                     new DistribuitorCandle { CandleID = candles.Single(c => c.Name == "Căsnicie" ).ID,DistribuitorID = distribuitors.Single(i => i.DistribuitorName =="Casa cu fericire").ID},
                     new DistribuitorCandle { CandleID = candles.Single(c => c.Name == "Iubire" ).ID,DistribuitorID = distribuitors.Single(i => i.DistribuitorName =="Nunta de vis").ID},

            };
            foreach (DistribuitorCandle pl in distribuitorcandles)
            {
                context.DistribuitorCandles.Add(pl);
            }
            context.SaveChanges();
        }
    }
}

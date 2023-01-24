using Newtonsoft.Json;
using System.Globalization;

namespace TaskMCA
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                List<Root> productList = GetProducts();

                var productListDomestic = productList!
                    .Where(x => x.domestic)
                    .OrderBy(x => x.name)
                    .Select(x => x)
                    .ToList();

                var productListImported = productList!
                    .Where(x => !x.domestic)
                    .OrderBy(x => x.name)
                    .Select(x => x)
                    .ToList();

                DisplayProducts(productListDomestic, productListImported);

            }
            catch (Exception)
            {
                Console.WriteLine("Error while retrieving product data");
            }

        }


        private static List<Root> GetProducts()
        {

            var url = "https://interview-task-api.mca.dev/qr-scanner-codes/alpha-qr-gFpwhsQ8fkY1";

            var productList = new List<Root>();

            using (var http = new HttpClient())
            {
                var endpoint = url;
                var result = http.GetAsync(endpoint).Result;
                var json = result.Content.ReadAsStringAsync().Result;
                productList = JsonConvert.DeserializeObject<List<Root>>(json);
                if (productList != null)
                {
                    return productList;
                }
                else
                {
                    throw new Exception();
                }

            }

        }



        private static void DisplayProducts(List<Root> productListDomestic, List<Root> productListImported)
        {

            Console.OutputEncoding = System.Text.Encoding.Unicode;

            Console.WriteLine(".Domestic");
            DisplayData(productListDomestic);


            Console.WriteLine(".Imported");
            DisplayData(productListImported);


            Console.WriteLine($"Domestic cost: {productListDomestic.Sum(x => x.price).ToString("C1", CultureInfo.CreateSpecificCulture("en-US"))}");
            Console.WriteLine($"Imported cost: {productListImported.Sum(x => x.price).ToString("C1", CultureInfo.CreateSpecificCulture("en-US"))}");


            Console.WriteLine($"Domestic count: {productListDomestic.Count}");
            Console.WriteLine($"Imported count: {productListImported.Count}");

        }


        private static void DisplayData(List<Root> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine("...." + item.name);
                Console.WriteLine("    Price: " + item.price.ToString("C1", CultureInfo.CreateSpecificCulture("en-US")));
                if (item.description != null)
                {
                    Console.WriteLine("    " + item.description.Substring(0, 10) + "...");
                }

                Console.Write("    Weight: ");
                if (Convert.ToInt32(item.weight) == 0)
                {
                    Console.WriteLine("N/A");
                }
                else
                {
                    Console.WriteLine("{0}g", item.weight);
                }
            }
        }

    }

}




















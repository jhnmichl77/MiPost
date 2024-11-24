using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Globalization;
using Spectre.Console;
namespace NewFinalProject
{
    internal interface IStaffRole
    {
        void DisplayInventory(List<Product> products);
        void Checkout(List<Product> products, ProductFile productFile, SalesFile salesFile);
        void AddProduct(List<Product> products, ProductFile productFile);
        void SearchProduct(List<Product> products);

    }
    internal interface IManagerRole : IStaffRole
    {
        void EditProduct(List<Product> products);
        void DeleteProduct(List<Product> products, ProductFile productFile);
    }
    internal class LogIn
    {
        public string ID { get; private set; }
        public string Name { get; set; }
        public string Password { get; private set; }
        public string Role { get; set; }

        public LogIn(string id, string name, string password, string role)
        {
            ID = id;
            Name = name;
            Password = password;
            Role = role;
        }

        public bool ValidateLogin(string id, string password)
        {
            return ID == id && Password == password;
        }

        public override string ToString()
        {
            return $"{ID},{Name},{Password},{Role}";
        }
    }
    internal class Manager : LogIn, IManagerRole
    {
        public Manager(string id, string name, string password, string role) : base(id, name, password, role) { }
        public void DisplayInventory(List<Product> products)
        {
            
            SharedProductActions.DisplayInventory(products);

        }
        public void AddProduct(List<Product> products, ProductFile productFile)
        {
            SharedProductActions.AddProduct(products, productFile);
            
            
        }
        public void EditProduct(List<Product> products)
        {
            Console.WriteLine("Edit Product Details");
            Console.Write("Product ID: ");
            string EProductID = Console.ReadLine();
            Product product = null;
            for (int i = 0; i < products.Count; i++)
            {
                if (products[i].ProductID == EProductID)
                {
                    product = products[i];
                    break;
                }
            }
            if (product == null)
            {
                Console.Clear();
                Console.Write("Product not found!");
                Thread.Sleep(1500);
                Console.Clear();
                return;
            }
            Console.Clear();
            Console.WriteLine("Which do you want to edit?");
            Console.WriteLine("[1] Product ID");
            Console.WriteLine("[2] Name");
            Console.WriteLine("[3] Price");
            Console.WriteLine("[4] Quantity");
            Console.WriteLine("[5] Manufacturing Date");
            Console.WriteLine("[6] Expiry Date");
            Console.Write("Input: ");
            int EPChoice = int.Parse(Console.ReadLine());

            switch (EPChoice)
            {
                case 1:
                    Console.Clear();
                    Console.Write("New Product ID: ");
                    string NewProductID = Console.ReadLine();
                    product.ProductID = NewProductID;
                    break;
                case 2:
                    Console.Clear();
                    Console.Write("New Product Name: ");
                    string NewProductName = Console.ReadLine();
                    product.ProductName = NewProductName;
                    break;
                case 3:
                    Console.Clear();
                    Console.WriteLine("New producut Price: ");
                    try
                    {
                        float NewProductPrice = float.Parse(Console.ReadLine());
                        if (NewProductPrice <= 0)
                        {
                            throw new ArgumentOutOfRangeException("Invalid, Price Cannot Be less than or equal ZERO!");

                        }
                        product.Price = NewProductPrice;
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.Clear();
                        Console.WriteLine(e.Message);
                        Thread.Sleep(1500);
                        Console.Clear();
                        return;
                    }
                    catch (FormatException)
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid input, Please Enter a numeric value");
                        Thread.Sleep(1500);
                        Console.Clear();
                    }

                    break;
                case 4:
                    Console.Clear();
                    Console.Write("New Quantity: ");
                    try
                    {
                        int NewProductQuantity = int.Parse(Console.ReadLine());
                        if (NewProductQuantity <= 0)
                        {
                            throw new ArgumentOutOfRangeException("Invalid, Quantity cannot be less than zero");
                        }
                        product.Quantity = NewProductQuantity;

                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.Clear();
                        Console.WriteLine(e.Message);
                        Thread.Sleep(1500);
                        Console.Clear();
                        return;
                    }
                    catch (FormatException)
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid input, Please Enter a numeric value");
                        Thread.Sleep(1500);
                        Console.Clear();
                    }
                    break;
                case 5:
                    Console.Clear();
                    Console.Write("New Manufacturing Date mm/dd/yyyy: ");
                    string NewMfgDate = Console.ReadLine();
                    product.ProductMfgDate = NewMfgDate;
                    break;
                case 6:
                    Console.Clear();
                    Console.Write("New Exipry Date mm/dd/yyyy: ");
                    string NewExpDate = Console.ReadLine();
                    product.ProductExpDate = NewExpDate;
                    break;
                default:
                    Console.WriteLine("Invalid Input");
                    break;
            }
            Console.Clear();
        }
        public void DeleteProduct(List<Product> products, ProductFile productFile)
        {
            Console.Clear();
            SharedProductActions.DisplayInventory(products);
            Console.Write("Enter Product ID to delete: ");
            string DProductID = Console.ReadLine();
            bool productFound = false;

            for (int i = 0; i < products.Count; i++)
            {
                if (products[i].ProductID == DProductID)
                {
                    products.RemoveAt(i);
                    productFile.SaveProducts(products);
                    productFound = true;
                    Console.Clear();
                    Console.Write("Product deleted successfully.");
                    Thread.Sleep(1500);
                    Console.Clear();
                    break;
                }
            }

            if (!productFound)
            {
                Console.Clear();
                Console.WriteLine("Product not found. Please check the Product ID.");
                Thread.Sleep(1500);
                Console.Clear();
            }
            Console.Clear();
        }
        public void Checkout(List<Product> products, ProductFile productFile, SalesFile salesFile)
        {
            SharedProductActions.Checkout(products, productFile, salesFile);
        }
        public void SearchProduct(List<Product> products)
        {
            SharedProductActions.SearchProduct(products);

        }
        public void GenerateReport(SalesFile salesFile)
        {
            Console.Clear();
            Console.WriteLine("Generate Sales Report");
            Console.WriteLine("[1] Daily");
            Console.WriteLine("[2] Weekly");
            Console.WriteLine("[3] Monthly");
            Console.WriteLine("[5] Yearly");
            Console.Write("Input: ");

            try
            {
                int choice = int.Parse(Console.ReadLine());

                if (choice < 1 || choice > 4)
                {
                    Console.WriteLine("Invalid Input");
                    return;
                }

                List<string> sales = salesFile.LoadSales();
                float totalSales = 0;
                List<string> filteredSales = new List<string>();
                DateTime currentDate = DateTime.Now;

                DateTime specificDate = default;
                DateTime parsedMonthYear = default;
                int specificYear = 0;

                switch (choice)
                {
                    case 1: 
                        Console.Write("Enter Specific Date (mm/dd/yyyy): ");
                        string specificDay = Console.ReadLine();
                        if (!DateTime.TryParseExact(specificDay, "MM/dd/yyyy", null, DateTimeStyles.None, out specificDate))
                        {
                            Console.WriteLine("Invalid date format! Please try again.");
                            return;
                        }
                        break;
                    case 2: 
                        Console.Write("Enter Starting Date (mm/dd/yyyy): ");
                        string weeklyStartDateInput = Console.ReadLine();
                        if (!DateTime.TryParseExact(weeklyStartDateInput, "MM/dd/yyyy", null, DateTimeStyles.None, out specificDate))
                        {
                            Console.WriteLine("Invalid date format! Please try again.");
                            return;
                        }
                        break;
                    case 3: 
                        Console.Write("Enter Month and Year (mm/yyyy): ");
                        string monthYear = Console.ReadLine();
                        if (!DateTime.TryParseExact(monthYear, "MM/yyyy", null, DateTimeStyles.None, out parsedMonthYear))
                        {
                            Console.WriteLine("Invalid Month/Year format! Please try again.");
                            return;
                        }
                        break;
                    case 4:
                        Console.Write("Enter Year (yyyy): ");
                        string yearInput = Console.ReadLine();
                        if (!int.TryParse(yearInput, out specificYear) || specificYear <= 0)
                        {
                            Console.WriteLine("Invalid Year! Please try again.");
                            return;
                        }
                        break;
                    default:
                        break; 
                }
                for (int i = 0; i < sales.Count; i++)
                {
                    string sale = sales[i];
                    string[] saleParts = sale.Split(',');

                    if (saleParts.Length == 3)
                    {
                        string timestamp = saleParts[1].Trim();
                        string grandTotalStr = saleParts[2].Trim();

                        if (DateTime.TryParse(timestamp, out DateTime saleDate) && float.TryParse(grandTotalStr, out float grandTotal))
                        {
                            bool validate = false;

                            switch (choice)
                            {
                                case 1: 
                                    if (saleDate.Date == specificDate.Date)
                                    {
                                        validate = true;
                                    }
                                    break;
                                case 2:
                                    if (saleDate >= specificDate && saleDate < specificDate.AddDays(7))
                                    {
                                        validate = true;
                                    }
                                    break;
                                case 3: 
                                    if (saleDate.Month == parsedMonthYear.Month && saleDate.Year == parsedMonthYear.Year)
                                    {
                                        validate = true;

                                    }
                                    break;

                                case 4: 
                                    if (saleDate.Year == specificYear)
                                    {
                                        validate = true;
                                    }
                                    break;

                                default:
                                    break;
                            }

                            if (validate)
                            {
                                filteredSales.Add(sale);
                                totalSales += grandTotal;
                            }
                        }
                    }
                }

                string time;
                if (choice == 1)
                {
                    time = "Daily";
                }
                else if (choice == 2)
                {
                    time = "Weekly";
                }
                else if (choice == 3)
                {
                    time = "Monthly";
                }
                else
                {
                    time = "Yearly";
                }

                Console.WriteLine($"{time} Sales Report");
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine($"Total Sales: Php {totalSales:F2}");
                Console.WriteLine("-------------------------------------------");
                for (int i = 0; i < filteredSales.Count; i++)
                {
                    Console.WriteLine(filteredSales[i]);
                }
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine($"Error: {e.Message}");
                Thread.Sleep(1500);
                Console.Clear();
            }
        }


    }
    internal class Monitor
    {
        public static void QuantityChecker(List<Product> products, int Low = 30)
        {
            for (int i = 0; i < products.Count; i++)
            {
                if (products[i].Quantity <= Low)
                {
                    Console.WriteLine("Warning");
                    Console.WriteLine($"{products[i].ProductName}'s Quantity is Low, {products[i].Quantity} Left, Time to restock!");
                }
            }
        }
        public static void ExpiryDateChecker(List<Product> products, int days = 5)
        {
            for (int i = 0; i < products.Count; i++)
            {
                Product product = products[i];

                DateTime? expiryDate = GetExpiryDate(product.ProductExpDate);

                if (expiryDate.HasValue)
                {
                    if ((expiryDate.Value - DateTime.Now).Days <= days)
                    {
                        Console.WriteLine($"{product.ProductName} is nearing its expiry date,Expires on {expiryDate.Value.ToShortDateString()}.");
                    }
                }
            }
        }
        public static DateTime? GetExpiryDate(string expiryDateString)
        {
            if (DateTime.TryParse(expiryDateString, out DateTime parsedDate))
            {
                return parsedDate;
            }
            else
            {
                return null;
            }
        }
    }
    internal class Product
    {
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public string ProductMfgDate { get; set; }
        public string ProductExpDate { get; set; }

        public Product(string productid, string productname, float price, int quantity, string productmfgdate, string productexpdate)
        {
            ProductID = productid;
            ProductName = productname;
            Price = price;
            Quantity = quantity;
            ProductMfgDate = productmfgdate;
            ProductExpDate = productexpdate;
        }
        public override string ToString()
        {
            return $"{ProductID},{ProductName},{Price},{Quantity},{ProductMfgDate},{ProductExpDate}";
        }
    }
    internal class SharedProductActions
    {
        public static void DisplayInventory(List<Product> products)
        {
            var table = new Table();
            table.AddColumn("Product ID");
            table.AddColumn("Product Name");
            table.AddColumn("Quantity");
            table.AddColumn("Price");

            for (int i = 0; i < products.Count; i++)
            {
                table.AddRow(
                    products[i].ProductID,
                    products[i].ProductName,
                    products[i].Quantity.ToString(),
                    products[i].Price.ToString("F2")
                );
            }

            AnsiConsole.Write(table);
        }

        public static void AddProduct(List<Product> products, ProductFile productFile)
        {
            Console.WriteLine("Enter Product Details.");
            Console.Write("Product ID: ");
            string productId = Console.ReadLine();
            for (int i = 0; i < products.Count; i++)
            {
                if (productId == products[i].ProductID)
                {
                    Console.Clear();
                    Console.Write("A product with similar ID already Exist.");
                    Thread.Sleep(1500);
                    Console.Clear();
                    return;
                }
            }
            Console.Write("Product Name: ");
            string productName = Console.ReadLine();
            Console.Write("Price: ");
            float price = float.Parse(Console.ReadLine());
            try
            {
                if (price <= 0)
                {
                    throw new ArgumentOutOfRangeException("Invalid, Price Cannot Be less than or equal ZERO!");

                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                Thread.Sleep(1500);
                Console.Clear();
                return;
            }
            Console.Write("Quantity: ");
            int quantity = int.Parse(Console.ReadLine());
            try
            {
                if (quantity <= 0)
                {
                    throw new ArgumentOutOfRangeException("Invalid, Quantity cannot be less than zero");
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                Thread.Sleep(1500);
                Console.Clear();
                return;
            }
            Console.Write("Manufacturing Date(mm/dd/yy): ");
            string mfgdate = Console.ReadLine();
            if (string.IsNullOrEmpty(mfgdate))
            {
                mfgdate = "N/A";
            }

            Console.Write("Expiration Date(mm/dd/yy): ");
            string expdate = Console.ReadLine();
            if (string.IsNullOrEmpty(expdate))
            {
                expdate = "N/A";
            }
            Product NewProduct = new Product(productId, productName, price, quantity, mfgdate, expdate);
            products.Add(NewProduct);
            productFile.SaveProducts(products);
            Console.Clear();
            Console.WriteLine("Product Added Succesfully!");
            Thread.Sleep(1500);
            Console.Clear();
        }
        public static void Checkout(List<Product> products, ProductFile productFile, SalesFile salesFile)
        {
            Console.Clear();
            DisplayInventory(products);

            List<string> cart = new List<string>();
            List<float> lineTotals = new List<float>();
            float grandTotal = 0;
            Console.WriteLine("Cart:");

            // Add products to cart
            while (true)
            {
                Console.Write("Enter Product ID: ");
                string cProductID = Console.ReadLine();
                bool found = false;

                for (int i = 0; i < products.Count; i++)
                {
                    if (products[i].ProductID == cProductID)
                    {
                        found = true;
                        Console.WriteLine($"You are ordering: {products[i].ProductName}");
                        Console.Write("Quantity: ");
                        int cQuantity = int.Parse(Console.ReadLine());

                        if (cQuantity <= 0)
                        {
                            Console.WriteLine("Invalid order quantity!");
                            return;
                        }

                        if (cQuantity > products[i].Quantity)
                        {
                            Console.WriteLine("Insufficient stock! Please reduce the quantity.");
                            return;
                        }

                        float lineTotal = products[i].Price * cQuantity;
                        cart.Add($"{products[i].ProductName}\t{cQuantity}\t{products[i].Price:F2}\t{lineTotal:F2}");
                        lineTotals.Add(lineTotal);
                        products[i].Quantity -= cQuantity;
                        break;
                    }
                }

                if (!found)
                {
                    Console.WriteLine("Product not found! Please try again.");
                }

                Console.Write("Do you want to add another product [y/n]?: ");
                string option = Console.ReadLine().ToUpper();
                if (option != "Y")
                {
                    break;
                }
            }

            Console.Clear();
            Console.WriteLine("\t\tYour Receipt:");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Product Name\tQuantity\tPrice\tLine Total");
            Console.WriteLine("-------------------------------------------");
            foreach (string item in cart)
            {
                Console.WriteLine(item);
            }

            foreach (float lineTotal in lineTotals)
            {
                grandTotal += lineTotal;
            }

            Console.WriteLine("-------------------------------------------");
            Console.WriteLine($"Grand Total: {grandTotal:F2}");

            Payment payment = GetPaymentMethod(grandTotal);
            payment.ProcessPayment(grandTotal);

            productFile.SaveProducts(products);

            Guid uniqueId = Guid.NewGuid();
            string idString = uniqueId.ToString();
            DateTime currentDate = DateTime.Now;
            string formattedDate = currentDate.ToString("MM/dd/yyyy");
            string record = $"{idString},{currentDate},{grandTotal}";
            salesFile.SaveSales(record);

            Console.WriteLine("Thank you for shopping with MiPOST!");
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
            Console.Clear();
        }
        public static Payment GetPaymentMethod(float totalAmount)
        {
            Console.WriteLine("Select payment method:");
            Console.WriteLine("1. Cash");
            Console.WriteLine("2. Credit Card");
            Console.WriteLine("3. Digital Wallet");

            int choice = int.Parse(Console.ReadLine());
            Payment payment;

            switch (choice)
            {
                case 1:
                    payment = new CashPayment();
                    break;
                case 2:
                    payment = new CreditCardPayment();
                    break;
                case 3:
                    payment = new DigitalWalletPayment();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Defaulting to cash payment.");
                    payment = new CashPayment();
                    break;
            }

            return payment;
        }



        public static void SearchProduct(List<Product> products)
        {
            Console.Clear();
            bool found = false;

            do
            {
                Console.Write("Search Product ID: ");
                string SProductID = Console.ReadLine();
                string ans;
                for (int i = 0; i < products.Count; i++)
                {
                    if (SProductID == products[i].ProductID)
                    {
                        Console.WriteLine("--------------------------------------------Product Information--------------------------------------------");
                        Console.WriteLine("Name\t\t||Price\t\t||Quantity\t||Manufacturing Date\t||Expiry Date");
                        Console.WriteLine($"{products[i].ProductName}\t{products[i].Price}\t\t{products[i].Quantity}\t\t{products[i].ProductMfgDate}\t\t{products[i].ProductExpDate}");
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    Console.Clear();
                    Console.WriteLine("Product Not Found");
                    Thread.Sleep(2000);
                    Console.Clear();
                    return;
                }
                Console.Write("Do you want to search another product[y/n]?: ");
                ans = Console.ReadLine().ToLower();
                if (ans == "y")
                {
                    found = false;
                }
                else
                {
                    break;
                }
                Console.Clear();
            } while (true);

            Console.Clear();
        }
    }
    public abstract class Payment
    {
        public abstract void ProcessPayment(float amount);
    }
    public class CashPayment : Payment
    {
        public override void ProcessPayment(float totalAmount)
        {
            float cash = 0, change = 0;
            while (true)
            {
                Console.Write("Enter cash amount: ");
                cash = float.Parse(Console.ReadLine());

                if (cash <= 0)
                {
                    Console.WriteLine("Invalid cash amount! Please enter a valid positive number.");
                    Thread.Sleep(1500);
                    continue;
                }
                if (cash < totalAmount)
                {
                    Console.WriteLine("Insufficient funds! Please enter an amount greater than or equal to the total amount.");
                    Thread.Sleep(1500);
                    continue;
                }
                change = cash - totalAmount;
                Console.WriteLine($"Payment successful. Change: {change:F2}");
                break;
            }
        }
    }

    public class CreditCardPayment : Payment
    {
        public override void ProcessPayment(float totalAmount)
        {
            Console.WriteLine($"Processing credit card payment for {totalAmount:F2}...");
            Console.Write("Enter card number: ");
            string cardNumber = Console.ReadLine();
            Console.WriteLine("Payment successful.");
        }
    }

    public class DigitalWalletPayment : Payment
    {
        public override void ProcessPayment(float totalAmount)
        {
            Console.WriteLine($"Processing digital wallet payment for {totalAmount:F2}...");
            Console.Write("Enter wallet ID: ");
            string walletId = Console.ReadLine();
            Console.WriteLine("Payment successful.");
        }
    }

    internal class Staff : LogIn, IStaffRole
    {
        public Staff(string id, string name, string password, string role) : base(id, name, password, role) { }

        public void DisplayInventory(List<Product> products)
        {
            SharedProductActions.DisplayInventory(products);
        }
        public void AddProduct(List<Product> products, ProductFile productFile)
        {
            SharedProductActions.AddProduct(products, productFile);
        }
        public void Checkout(List<Product> products, ProductFile productFile, SalesFile salesFile)
        {
            SharedProductActions.Checkout(products, productFile, salesFile);
        }
        public void SearchProduct(List<Product> products)
        {
            SharedProductActions.SearchProduct(products);

        }
    }
    internal class SalesFile
    {
        private string SalesFileName;
        public SalesFile(string salesfile)
        {
            SalesFileName = salesfile;
        }
        public void SaveSales(string record)
        {
            using(StreamWriter sf = new StreamWriter(SalesFileName, true))
            {
                sf.WriteLine(record);
            }
        }
        public List<string> LoadSales()
        {
            List<string> sales = new List<string>();
            try
            {
                if (File.Exists(SalesFileName))
                {
                    using (StreamReader sr = new StreamReader(SalesFileName))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            sales.Add(line);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("file not found");
                }
            }
            catch(IOException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                Thread.Sleep(1500);
                Console.Clear();
            }
            catch(Exception e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                Thread.Sleep(1500);
                Console.Clear();
            }
            return sales;
        }
    }
    internal class AccountFile
    {
        private string AccFile;

        public AccountFile(string accName)
        {
            this.AccFile = accName;
        }

        public void SaveAccounts(List<LogIn> login)
        {
            using (StreamWriter sw = new StreamWriter(AccFile))
            {
                for (int i = 0; i < login.Count; i++)
                {
                    sw.WriteLine(login[i].ToString());
                }
            }
        }

        public List<LogIn> LoadAccounts()
        {
            List<LogIn> login = new List<LogIn>();
            try
            {
                if (File.Exists(AccFile))
                {
                    using (StreamReader sr = new StreamReader(AccFile))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] parts = line.Split(',');
                            if (parts.Length == 4)
                            {
                                login.Add(new LogIn(parts[0], parts[1], parts[2], parts[3]));
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No account file found");
                }
            }
            catch (IOException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                Thread.Sleep(1500);
                Console.Clear();
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                Thread.Sleep(1500);
                Console.Clear();
            }
            return login;
        }
    }
    internal class ProductFile
    {
        private string ProdFile;

        public ProductFile(string ProdName)
        {
            this.ProdFile = ProdName;
        }
        public void SaveProducts(List<Product> products)
        {
            using (StreamWriter psw = new StreamWriter(ProdFile))
            {
                for (int i = 0; i < products.Count; i++)
                {
                    psw.WriteLine(products[i].ToString());
                }
            }
        }
        public List<Product> LoadProducts()
        {
            List<Product> products = new List<Product>();
            try
            {
                if (File.Exists(ProdFile))
                {
                    using (StreamReader psr = new StreamReader(ProdFile))
                    {
                        string pline;
                        while ((pline = psr.ReadLine()) != null)
                        {
                            string[] pparts = pline.Split(',');
                            if (pparts.Length == 6)
                            {
                                products.Add(new Product(pparts[0], pparts[1], float.Parse(pparts[2]), int.Parse(pparts[3]), pparts[4], pparts[5]));
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No File for Products Found");
                }
            }
            catch (IOException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                Thread.Sleep(1500);
                Console.Clear();
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                Thread.Sleep(1500);
                Console.Clear();
            }
            return products;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            string AccFileName = "SavedAccounts.txt";
            string ProdFileName = "SavedProducts.txt";
            AccountFile handler = new AccountFile(AccFileName);
            ProductFile PHandler = new ProductFile(ProdFileName);
            SalesFile salesFile = new SalesFile("SavedSales.txt");
            List<LogIn> login = handler.LoadAccounts();
            List<Product> products = PHandler.LoadProducts();
            bool isLoggedIn = false;
            LogIn CurrentUser = null;
            while (!isLoggedIn)
            {
                Console.WriteLine("Welcome to MiPOST");
                Console.WriteLine("[1] LogIn");
                Console.WriteLine("[2] Register");
                Console.WriteLine("[3] Exit");
                Console.Write("Input: ");
                try
                {
                    int choice = int.Parse(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            isLoggedIn = Login(login, out CurrentUser);
                            if (isLoggedIn)
                            {
                                MainMenu(CurrentUser, products, PHandler, salesFile); ;
                            }
                            break;
                        case 2:
                            RegisterUser(login);
                            handler.SaveAccounts(login);
                            break;
                        case 3:
                            Console.Clear();
                            Console.WriteLine("Thank you for using MiPOST");
                            Thread.Sleep(1500);
                            Environment.Exit(0);
                            return;
                        default:
                            Console.Clear();
                            Console.WriteLine("Invalid Option.");
                            Thread.Sleep(1000);
                            Console.Clear();
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine(e.Message);
                    Thread.Sleep(1000);
                    Console.Clear();
                }
            }
        }
        static void RegisterUser(List<LogIn> login)
        {
            Console.WriteLine("Register a new account.");
            try
            {
                Console.Write("Enter ID: ");
                string id = Console.ReadLine();
                bool duplicate = false;
                for (int i = 0; i < login.Count; i++)
                {
                    if (id == login[i].ID)
                    {
                        Console.Clear();
                        Console.Write("ID Already exist");
                        Thread.Sleep(1500);
                        Console.Clear();
                        duplicate = true;
                        return;
                    }
                }
                if (duplicate)
                {
                    Console.Clear();
                    return;
                }
                Console.Write("Enter Name: ");
                string name = Console.ReadLine();

                Console.Write("Enter Password: ");
                string password = Console.ReadLine();

                Console.Write("Enter Role (manager/staff): ");
                string role = Console.ReadLine().ToLower();

                bool managerExists = false;
                for(int i = 0; i < login.Count; i++)
                {
                    if (login[i].Role == "manager")
                    {
                        managerExists = true;
                        break;
                    }
                }
                if (managerExists && role == "manager")
                {
                    Console.Clear();
                    Console.Write("A manager is already registered!");
                    Thread.Sleep(1500);
                    Console.Clear();
                }
                else
                {
                    login.Add(new LogIn(id, name, password, role));
                    Console.Clear();
                    Console.Write("Account Created Successfully!");
                    Thread.Sleep(1500);
                    Console.Clear();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        static bool Login(List<LogIn> login, out LogIn currentUser)
        {
            currentUser = null;
            try
            {
                Console.Clear();
                Console.WriteLine("Login to your account.");
                Console.Write("Enter ID: ");
                string inputID = Console.ReadLine();
                Console.Write("Enter Password: ");
                string inputPass = string.Empty;
                ConsoleKey key;
                do
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                    key = keyInfo.Key;
                    if (key == ConsoleKey.Backspace && inputPass.Length > 0)
                    {
                        inputPass = inputPass.Substring(0, inputPass.Length - 1);
                        Console.Write("\b \b");
                    }
                    else if (!char.IsControl(keyInfo.KeyChar))
                    {
                        inputPass += keyInfo.KeyChar;
                        Console.Write('*');
                    }
                } while (key != ConsoleKey.Enter);
                for (int i = 0; i < login.Count; i++)
                {
                    if (login[i].ValidateLogin(inputID, inputPass))
                    {
                        currentUser = login[i];
                        Console.Clear();
                        Console.Write("Login Successful");
                        Thread.Sleep(1000);
                        Console.Clear();
                        return true;
                    }
                }
                Console.Clear();
                Console.WriteLine("Login failed. Please check your credentials.");
                Thread.Sleep(1000);
                Console.Clear();
                return false;
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                Thread.Sleep(1000);
                Console.Clear();
                return false;
            }
        }
        static void MainMenu(LogIn user, List<Product> products, ProductFile productFile, SalesFile salesFile)
        {
            IStaffRole role;
            if (user.Role == "manager")
            {
                role = new Manager(user.ID, user.Name, user.Password, user.Role);
            }
            else
            {
                role = new Staff(user.ID, user.Name, user.Password, user.Role);
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Welcome, {user.Name}");
                Console.WriteLine("[1] Display Inventory");
                Console.WriteLine("[2] Add New Product");

                if (user.Role == "manager")
                {
                    Console.WriteLine("[3] Edit Product Details");
                    Console.WriteLine("[4] Delete Product From Inventory");
                }
                Console.WriteLine("[5] Checkout");
                Console.WriteLine("[6] Search Product");
                if(user.Role == "manager")
                {
                    Console.WriteLine("[7] Sales Reports");
                }
                Console.WriteLine("[8] Exit");
                Monitor.ExpiryDateChecker(products);
                Monitor.QuantityChecker(products);
                Console.Write("Select an option: ");
                try
                {
                    int choice = int.Parse(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            Console.Clear();
                            role.DisplayInventory(products);
                            Console.Write("Press any key to continue");
                            Console.ReadKey();
                            break;
                        case 2:
                            Console.Clear();
                            role.AddProduct(products, productFile);
                            break;
                        case 3:
                            if (role is Manager manager)
                            {
                                Console.Clear();
                                role.DisplayInventory(products);
                                manager.EditProduct(products);
                                productFile.SaveProducts(products);
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Access denied. Staff cannot edit products.");
                                Thread.Sleep(1500);
                                Console.Clear();
                            }
                            break;
                        case 4:
                            if (role is IManagerRole managerUser)
                            {
                                managerUser.DeleteProduct(products, productFile);
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Access denied. Only managers can delete products.");
                            }
                            break;
                        case 5:
                            role.Checkout(products, productFile, salesFile);
                            break;
                        case 6:
                            role.SearchProduct(products);
                            break;
                        case 7:
                            if (role is Manager managerRole)
                            {
                                try
                                {
                                    managerRole.GenerateReport(salesFile);
                                    Console.WriteLine("Press any key to continue");
                                    Console.ReadKey();
                                }
                                catch (Exception ex)
                                {
                                    Console.Clear();
                                    Console.WriteLine($"Error loading sales data: {ex.Message}");
                                    Thread.Sleep(1500);
                                    Console.Clear();
                                }
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Access denied. Only managers can generate sales reports.");
                                Thread.Sleep(1500);
                                Console.Clear();
                            }
                            break;
                        case 8:
                            Console.Clear();
                            Console.WriteLine("Thank you for using MiniPOST");
                            Thread.Sleep(1500);
                            Console.Clear();
                            Environment.Exit(0);
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("Invalid Option. Please try again.");
                            Thread.Sleep(1500);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine($"Error: {e.Message}");
                    Thread.Sleep(1500);
                    Console.Clear();
                }
            }
        }

    }
}
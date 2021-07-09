using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrentUserNamespace;
using P0DbContext.Models;
using System.Data.Entity.Core.Objects;

namespace P0Main
{
    class BuissnessModel : IBuissnessModel
    {
        public CurrentUser user;
        public P0DbClass context;

        /// <summary>
        /// Buissness Model Constructor
        /// </summary>
        /// <param name="user">object for the current user class</param>
        /// <param name="context">object for the database class</param>
        public BuissnessModel(CurrentUser user, P0DbClass context)
        {
            this.user = user;
            this.context = context;
        }

        /// <summary>
        /// Proviedes the Iinitial startup menu options and the switch case for them
        /// </summary>
        public void Startup()
        {

            // Greet the user and begin login process
            Console.Clear();
            Console.WriteLine("Welcome To Mason Sanborn's Project 0!");
            Console.WriteLine("To Begin please:");
            Console.WriteLine("\t1 : Log in");
            Console.WriteLine("\t2 : Create a new account");
            Console.WriteLine("\t3 : Search for a Customer");
            Console.WriteLine("\t4 : Display all order history of store location");
            Console.WriteLine("\t5 : Exit Program");

            // Allow the user to either log in or register a new account.
            int numChoices = 5;
            int userInput = user.getUserInputInt(1, numChoices);
            switch (userInput)
            {
                case 1:
                    Login();
                    break;
                case 2:
                    Register();
                    break;
                case 3:
                    SearchCustomer();
                    break;
                case 4:
                    DisplayLocationOrderHistory();
                    break;
                case 5:
                    Environment.Exit(0);
                    break;
            }
            // After the user has successfully logged in startup is complete, continue to main program functionality
            
            return;

        }

        /// <summary>
        /// User login fucntionality
        /// </summary>
        private void Login()
        {
            while (true)
            {
                Console.Clear();
                Console.Write("To login please enter your:\n\tusername: ");
                string usernameInput = Console.ReadLine();
                Console.Write("\tpassword: ");
                string passwordInput = Console.ReadLine();

                var loginUser = context.Customers.Where(x => x.UserName == usernameInput && x.Password == passwordInput).FirstOrDefault();

                if (loginUser == null)
                {
                    Console.WriteLine("No account found with that username / password...");
                    bool retryLogin = user.getUserInputYN("Would you like to try again? (y/n)");
                    if (retryLogin) continue;
                    else Register();
                    return;
                }
                else
                {
                    Console.Clear();
                    user.currentCustomer = loginUser;
                    Console.WriteLine($"Hello {user.currentCustomer.FirstName}, you have succesfully logged in!");
                    return;
                }

            }
        }

        /// <summary>
        /// Logic for registering a new user and adding them to the database
        /// </summary>
        private void Register()
        {
            string regFName;
            string regLName;
            string regUsername;
            string regPassword;

            Console.WriteLine("Lets get started creating your account!");
            do
            {
                Console.WriteLine("Please input your information using (a-z | A-Z | 0-9 | _ | .): ");
                do
                {
                    regFName = user.getUserInputString("\tFirst Name: ");
                } while (regFName == null || regFName == string.Empty);
                do
                {
                    regLName = user.getUserInputString("\tLast Name: ");
                } while (regLName == null || regLName == string.Empty);
                do
                {
                    regUsername = user.getUserInputString("\tUsername: ");
                } while (regUsername == null || regUsername == string.Empty);
                do
                {
                    regPassword = user.getUserInputString("\tPassword: ");
                } while (regPassword == null || regPassword == string.Empty);

                Console.WriteLine($"New Account-> First Name: {regFName}, Last Name: {regLName}, Username: {regUsername}, Password: {regPassword}");
            } while (!user.getUserInputYN("Is your info correct? (y/n)"));

            Customer newUser  = new Customer();
            newUser.FirstName = regFName;
            newUser.LastName  = regLName;
            newUser.UserName  = regUsername;
            newUser.Password  = regPassword;
            try
            {
                context.Add(newUser);     // add the new object to the database

                context.SaveChanges();      // save and update changes
            }
            catch
            {
                Console.WriteLine("There was an issue adding the 'New User' to the database!");
            };

            Console.WriteLine("New Account Registered!\n");
            Console.WriteLine("Press Enter to continue... ");
            Console.ReadLine();
            Login();
        }

        /// <summary>
        /// Allows the user to choose a new current location
        /// </summary>
        public void ChooseLocation()
        {
            Console.Clear();
            // order from users most commonly used store?
            Console.WriteLine("Please select which location you would like to shop from!");
            var LocationList = context.Locations.ToList();
            for (int i = 0; i < LocationList.Count; i++)
            {
                Console.WriteLine($"\t{(i)} : {LocationList[i].LocationName}");
            }
            int userLocationInput = user.getUserInputInt(0, LocationList.Count - 1);
            user.currentLocation = LocationList[userLocationInput];

            //Console.WriteLine($"Your chosen Location:\nname: {user.currentLocation.LocationName}, address : {user.currentLocation.LocationAddress}, id : {user.currentLocation.LocationId},");

            // The current users chosen Location has been set
            return;
        }

        /// <summary>
        /// display functions for main menu options that gets user input and returns the result
        /// </summary>
        /// <returns>int based on user choice</returns>
        public int MainMenuOptions()
        {
            int numOptions = 3;
            Console.Clear();
            Console.WriteLine("What would you like to do?");
            Console.WriteLine($"\t1 : Shop at {user.currentLocation.LocationName}");
            Console.WriteLine("\t2 : Change locations");
            //Console.WriteLine($"\t3 : Display all order history of {user.currentLocation.LocationName}");
            //Console.WriteLine("\t3 : Logout");
            Console.WriteLine("\t3 : Logout");
            // display all order history of customer
            // display order history of store?
                // order history can be sorted by earliest / latest / cheapest / most expensive (+5)
            // display statistics based on order history (+5)
            // get a suggested order for the customer (+5)

            return user.getUserInputInt(1, numOptions);

        }

        /// <summary>
        /// Functionality menu to shop at a location
        /// </summary>
        public void ShopAtLocation()
        {
            Console.WriteLine($"Welcome to {user.currentLocation.LocationName}!");
            while(true)
            {
                int numOptions = 5;
                Console.Clear();
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("\t1 : Browse Products By Category");
                Console.WriteLine("\t2 : Browse All Products");
                Console.WriteLine("\t3 : Display My Cart");
                Console.WriteLine("\t4 : Checkout");
                Console.WriteLine("\t5 : Exit without Checking out");

                //Console.WriteLine("\t5 : Generate a suggested order for me");

                int userChoice = user.getUserInputInt(1, numOptions);
                switch(userChoice)
                {
                    case 1:
                        BrowseCategories();
                        break;
                    case 2:
                        BrowseProducts();
                        break;
                    case 3: // Display Cart
                        DisplayCart();
                        break;
                    case 4: // Checkout
                        checkoutCart();
                        break;
                    case 5: // Exit without checking out
                        if (user.getUserInputYN("Are you sure you would like to exit and abandon your cart? (y/n)"))
                        {
                            user.shoppingCart.Clear();
                            return;
                        }
                        break;

                }
            }
        }

        /// <summary>
        /// Displays the users shopping cart to the user and allows modification of the cart
        /// </summary>
        public void DisplayCart()
        {
            decimal sum = 0;
            Console.Clear();
            Console.WriteLine("---Items In your cart!---\n");
            Console.WriteLine($"\t{"Product",-12}   {"Amount",-10}     {"Price"}\n");
            foreach (var item in user.shoppingCart)
            {
                Console.WriteLine($"\t{item.Key.ProductName,-12} : {item.Value,-10}  :  ${(item.Key.Price * item.Value)}");
                sum += (item.Key.Price * item.Value);
            }
            Console.Write($"\nYour total price will be: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"${sum}\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\nPress Enter to continue...");
            Console.ReadLine();
        }

        /// <summary>
        /// displays all the products for a store
        /// </summary>
        public void BrowseProducts()
        {
            Console.Clear();
            Console.WriteLine("What items would you like to add to your cart?");
            var joinResults = context.Inventories.Join(
                context.Products,
                invent => invent.ProductId,
                prod => prod.ProductId,
                (invent, prod) => new
                {
                    ProductId = prod.ProductId,
                    ProductName = prod.ProductName,
                    ProductLocationID = invent.LocationId,
                    ProductAmount = invent.NumberProducts,
                    ProductDesc = prod.Description,
                    ProductPrice = prod.Price
                }
            );
            var productList = joinResults.Where(x => x.ProductLocationID == user.currentLocation.LocationId).ToList();

            //Console.WriteLine($"\n\t0 : Return To Store Menu ****");
            Console.WriteLine($"\n\t{"Id", -3}\t \t{"Product Name", -15}\t \t{"Amount Available", -16}  \t{"Price", -8}\t \t{"Description"}\n");


            for (int i = 1; i <= productList.Count; i++)
            {
                Console.WriteLine($"\t{(i),-3}\t:\t{productList[i - 1].ProductName,-15}\t:\t{productList[i - 1].ProductAmount,-16} :\t{productList[i - 1].ProductPrice,-8}\t:\t{productList[i - 1].ProductDesc}");
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n\t0 : Return To Store Menu");
            Console.ForegroundColor = ConsoleColor.White;


            int userChoice = user.getUserInputInt(0, productList.Count);
            if (userChoice == 0) return;
            AddToCart(productList[userChoice - 1].ProductId, productList[userChoice - 1].ProductAmount);

            // if user choice = 0
            // else add product from productlist[input - 1]
        }

        /// <summary>
        /// displays the products for a store filtered by category
        /// </summary>
        /// <param name="categoryType">The type of category to browse by.</param>
        public void BrowseProducts(string categoryType)
        {
            Console.Clear();
            Console.WriteLine("What items would you like to add to your cart?");
            var joinResults = context.Inventories.Join(
                context.Products,
                invent => invent.ProductId,
                prod => prod.ProductId,
                (invent, prod) => new
                {
                    ProductId = prod.ProductId,
                    ProductName = prod.ProductName,
                    ProductLocationId = invent.LocationId,
                    ProductAmount = invent.NumberProducts,
                    ProductDesc = prod.Description,
                    ProductCategory = prod.Category,
                    ProductPrice = prod.Price
                }
            );
            var productList = joinResults.Where(x => x.ProductLocationId == user.currentLocation.LocationId && x.ProductCategory == categoryType).ToList();

            //Console.WriteLine($"\t0 : Return To Store Menu ****");
            Console.WriteLine($"\n\t{"Id",-3}\t \t{"Product Name",-15}\t \t{"Amount Available",-16}  \t{"Price",-8}\t \t{"Description"}\n");

            for (int i = 1; i <= productList.Count; i++)
            {
                Console.WriteLine($"\t{(i),-3}\t:\t{productList[i - 1].ProductName,-15}\t:\t{productList[i - 1].ProductAmount,-16} :\t{productList[i - 1].ProductPrice,-8}\t:\t{productList[i - 1].ProductDesc}");
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n\t0 : Return To Store Menu");
            Console.ForegroundColor = ConsoleColor.White;

            int userChoice = user.getUserInputInt(0, productList.Count);
            if (userChoice == 0) return;
            AddToCart(productList[userChoice - 1].ProductId, productList[userChoice - 1].ProductAmount);
        }

        /// <summary>
        /// Allows the user to choose a category to shop from then calls the browse products for that category
        /// </summary>
        public void BrowseCategories()
        {
            Console.Clear();
            Console.WriteLine("\nPlease select which Category you would like to shop from!");

            var joinResults = context.Inventories.Join(
                context.Products,
                invent => invent.ProductId,
                prod => prod.ProductId,
                (invent, prod) => new
                {
                    ProductLocationId = invent.LocationId,
                    ProductCategory = prod.Category
                }

            );

            var categoryList = joinResults.Where(x => x.ProductLocationId == user.currentLocation.LocationId).Distinct().ToList();

            for (int i = 0; i < categoryList.Count; i++)
            {
                Console.WriteLine($"\t{i} : {categoryList[i].ProductCategory}");
            }

            int userChoice = user.getUserInputInt(0, categoryList.Count - 1);
            string categoryType = categoryList[userChoice].ProductCategory;
            BrowseProducts(categoryType);
        }

        /// <summary>
        /// Functionality to add a product to the users shopping cart
        /// </summary>
        /// <param name="productId">Product to be added</param>
        /// <param name="amountInStore">The number available in the store</param>
        public void AddToCart(int productId, int amountInStore)
        {

            int numInCart = 0;
            Product cartProduct = context.Products.Where(x => x.ProductId == productId).FirstOrDefault();

            if (user.shoppingCart.TryGetValue(cartProduct, out numInCart)) { }

            int maxAmount = amountInStore - numInCart;

            Console.Write($"How many {cartProduct.ProductName} would you like: (max : {maxAmount}) : ");
            int numAdded = user.getUserInputInt(0, maxAmount);

            if((cartProduct.Price * numAdded) > 50)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                if (!user.getUserInputYN($"Are you sure you would like to add ${(cartProduct.Price * numAdded)} worth of {cartProduct.ProductName} to your cart? (y/n)"))
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"\nThe {cartProduct.ProductName}(s) have not been added to your cart. Returning to store.");
                    Console.WriteLine("Press Enter to continue... ");
                    Console.ReadLine();
                    return;

                }
                Console.ForegroundColor = ConsoleColor.White;

            }

            if (numAdded == 0) return;

            if (user.shoppingCart.ContainsKey(cartProduct))
            {
                user.shoppingCart[cartProduct] += numAdded;
            }
            else
            {
                user.shoppingCart.Add(cartProduct, numAdded);
            }


            Console.WriteLine($"\nSuccessfully added {numAdded}, {cartProduct.ProductName}(s) to your cart!");
            Console.WriteLine("Press Enter to continue... ");
            Console.ReadLine();

        }

        /// <summary>
        /// Functionality to purchase the items in cart and update the database records
        /// </summary>
        public void checkoutCart()
        {
            // for each element in the shopping cart subrtract the number ordered for each productid from the context.inventory
            decimal sum = 0;
            Console.Clear();
            Console.WriteLine("---Items In your cart!---\n");
            Console.WriteLine($"\t{"Product",-12}   {"Amount",-10}     {"Price"}\n");
            foreach (var item in user.shoppingCart)
            {
                Console.WriteLine($"\t{item.Key.ProductName, -12}\t:\t{item.Value, -10}\t{(item.Key.Price * item.Value)}");
                sum += (item.Key.Price * item.Value);
            }
            Console.Write($"\nYour total price will be: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"${sum}\n");
            Console.ForegroundColor = ConsoleColor.White;

            if (!user.getUserInputYN("Are you sure you would like to checkout? (y/n)")) return;

            // Build the new order object for this customer at this location
            Order thisOrder = new Order();
            thisOrder.OrderTime = DateTime.Now;
            thisOrder.CustomerId = user.currentCustomer.CustomerId;
            thisOrder.LocationId = user.currentLocation.LocationId;
            try {
                // Add the new order object to the Database
                context.Add(thisOrder);
                context.SaveChanges();
            }
            catch
            {
                Console.WriteLine("There was an issue adding the new 'Order' to the database!");
            };
            Console.WriteLine($"new order: time: {thisOrder.OrderTime}, customer id: {thisOrder.CustomerId}, location id {thisOrder.LocationId}");
            var newOrderId = context.Orders.Max(x => x.OrderId);
            Console.WriteLine($"your order id: {newOrderId}");

            // Update the stores inventory in the databse 
            foreach (var item in user.shoppingCart)
            {
                foreach(var obj in context.Inventories)
                {
                    if(obj.LocationId == user.currentLocation.LocationId && obj.ProductId == item.Key.ProductId)
                    {
                        obj.NumberProducts -= item.Value;
                    }
                }
                // Add an ordered product object for each product in the shopping cart
                var newOrderedProduct = new OrderedProduct();
                newOrderedProduct.OrderId       = newOrderId;
                newOrderedProduct.ProductId     = item.Key.ProductId;
                newOrderedProduct.NumberOrdered = item.Value;
                context.Add(newOrderedProduct);
            }
            // Save Database Changes and clear user's shopping cart
            try
            {
            user.shoppingCart.Clear();
            context.SaveChanges();
            }
            catch
            {
                Console.WriteLine("There was an issue adding the 'Ordered Product' to the database!");
            };
            Console.WriteLine("\nYou have succesfully purchased your items!");
            Console.WriteLine("Press Enter to continue... ");
            Console.ReadLine();
            Startup();

        }

        /// <summary>
        /// allows for searching by any combination of customer id / first name / last name
        /// </summary>
        public void SearchCustomer()
        {
            // TODO add search protections to chosen user id to view order history
            Console.Clear();
            Console.WriteLine("Please Enter information about the user!");
            Console.WriteLine("\tLeave any unknown information blank!\n");
            string fName = string.Empty;
            string lName = string.Empty;
            string custId = string.Empty;
            int custIdInt = -1;
            int custOrderSearchId;

            Console.WriteLine("Please Enter information about the user!");

            custId = user.getUserInputString("\tCustomer Id: ");

            if (custId != string.Empty)
            {
                bool succesfulConversion = Int32.TryParse(custId, out custIdInt);

                // checks to see that the user entered a number and that the number is in range
                // if invalid input, succesfulConversion is false
                if (succesfulConversion != true)
                {
                    Console.WriteLine("Invalid User Id");
                    custIdInt = -1;
                }
            }
            if (custIdInt != -1)
            {
                var searchResult = context.Customers.Where(x => x.CustomerId == custIdInt);
                if (searchResult == null)
                {
                    Console.WriteLine("No results Found");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine($"{"Customer Id",-12} {"First Name",-12} {"Last Name",-15} {"UserName",-10}");
                    foreach (var obj in searchResult)
                    {
                        Console.WriteLine($"{obj.CustomerId,-12} {obj.FirstName,-12} {obj.LastName,-15} {obj.UserName,-10}");
                    }
                    if (user.getUserInputYN("Would you like to view the order history of a customer above? (y/n) "))
                    {
                        Console.Write("Please enter the Id: ");
                        custOrderSearchId = user.getUserInputInt(0, 9999);
                        DisplayCustomerOrderHistory(custOrderSearchId);
                    }
                }
            }
            else {
                fName = user.getUserInputString("\tFirst Name: ");
                lName = user.getUserInputString("\tLast Name: ");

                if (fName != string.Empty && lName != string.Empty)
                {
                    // if there is a first and last name query both
                    var searchResult = context.Customers.Where(x => x.FirstName == fName && x.LastName == lName);
                    if(searchResult == null)
                    {
                        Console.WriteLine("No results Found");
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"{"Customer Id", -12} {"First Name", -12} {"Last Name", -15} {"UserName", -10}");
                        foreach(var obj in searchResult)
                        {
                            Console.WriteLine($"{obj.CustomerId,-12} {obj.FirstName,-12} {obj.LastName,-15} {obj.UserName,-10}");
                        }
                        if (user.getUserInputYN("Would you like to view the order history of a customer above? (y/n) "))
                        {
                            Console.Write("Please enter the Id: ");
                            custOrderSearchId = user.getUserInputInt(0, 9999);
                            DisplayCustomerOrderHistory(custOrderSearchId);
                        }
                    }

                }
                else if (fName != string.Empty && lName == string.Empty)
                {
                    // if there is a first name but no last name
                    var searchResult = context.Customers.Where(x => x.FirstName == fName);
                    if (searchResult == null)
                    {
                        Console.WriteLine("No results Found");
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"{"Customer Id",-12} {"First Name",-12} {"Last Name",-15} {"UserName",-10}");
                        foreach (var obj in searchResult)
                        {
                            Console.WriteLine($"{obj.CustomerId,-12} {obj.FirstName,-12} {obj.LastName,-15} {obj.UserName,-10}");
                        }
                        if (user.getUserInputYN("Would you like to view the order history of a customer above? (y/n) "))
                        {
                            Console.Write("Please enter the Id: ");
                            custOrderSearchId = user.getUserInputInt(0, 9999);
                            DisplayCustomerOrderHistory(custOrderSearchId);
                        }
                    }
                }
                else if (fName == string.Empty && lName != string.Empty)
                {
                    // if there is a last name but no first name
                    var searchResult = context.Customers.Where(x => x.LastName == lName);
                    if (searchResult == null)
                    {
                        Console.WriteLine("No results Found");
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"{"Customer Id",-12} {"First Name",-12} {"Last Name",-15} {"UserName",-10}");
                        foreach (var obj in searchResult)
                        {
                            Console.WriteLine($"{obj.CustomerId,-12} {obj.FirstName,-12} {obj.LastName,-15} {obj.UserName,-10}");
                        }
                    }
                    if (user.getUserInputYN("Would you like to view the order history of a customer above? (y/n) "))
                    {
                        Console.Write("Please enter the Id: ");
                        custOrderSearchId = user.getUserInputInt(0, 9999);
                        DisplayCustomerOrderHistory(custOrderSearchId);
                    }

                }
                else
                {
                    Console.WriteLine("Insufficient information to provide a result.");
                }
            }




            Console.WriteLine("Returning to main menu");
            Console.WriteLine("Press Enter to continue... ");
            Console.ReadLine();
            Startup();
        }

        /// <summary>
        /// Displays the chosen customers order history
        /// </summary>
        /// <param name="custId">the id of the customer to display</param>
        public void DisplayCustomerOrderHistory(int custId)
        {
            Console.Clear();
            var searchResult = context.Orders.Where(x => x.CustomerId == custId);
            if(searchResult == null)
            {
                Console.WriteLine("This Customer has no order history!");
            }
            else
            {
                Console.WriteLine($"\t{"Order Id", -10} {"Customer Id", -12} {"Location Id", -16} {"Order Time"}\n");
                foreach(var obj in searchResult)
                {
                    Console.WriteLine($"\t{obj.OrderId,-10} {obj.CustomerId,-12} {obj.LocationId,-16} {obj.OrderTime}");
                }
                Console.WriteLine();
            }
            if (user.getUserInputYN("Would you like to view the order details of an order above? (y/n) "))
            {
                Console.Write("Please enter the Id: ");
                int orderSearchId = user.getUserInputInt(0, 9999);
                DisplayOrderDetails(orderSearchId);
            }

        }

        /// <summary>
        /// Displays order history from any location the user chooses
        /// </summary>
        public void DisplayLocationOrderHistory()
        {
            // TODO add user input protections for location id choosing
            Console.Clear();
            Console.WriteLine("Which Location would you like to view the order history of?\n");
            Console.WriteLine($"\t{"Location Id",-14} {"Location Name",-18}\n");

            foreach(var obj in context.Locations)
            {
                Console.WriteLine($"\t{obj.LocationId,-14} {obj.LocationName,-12}");
            }

            Console.Write("\n\tPlease enter the Id: ");
            int locId = user.getUserInputInt(0, 9999);

            Console.Clear();

            var searchResult = context.Orders.Where(x => x.LocationId == locId);
            if (searchResult == null)
            {
                Console.WriteLine("This Customer has no order history!");
            }
            else
            {
                Console.WriteLine($"\n\t{"Order Id",-10} {"Location Id",-12} {"Customer Id",-16} {"Order Time"}\n");
                foreach (var obj in searchResult)
                {
                    Console.WriteLine($"\t{obj.OrderId,-10} {obj.LocationId,-12} {obj.CustomerId,-16} {obj.OrderTime}");
                }
                Console.WriteLine();
            }

            if (user.getUserInputYN("Would you like to view the order details of an order above? (y/n) "))
            {
                Console.Write("Please enter the Id: ");
                int orderSearchId = user.getUserInputInt(0, 9999);
                DisplayOrderDetails(orderSearchId);
            }

            Console.WriteLine("Returning to main menu");
            Console.WriteLine("Press Enter to continue... ");
            Console.ReadLine();
            Startup();
        }

        /// <summary>
        /// Displays the information about an order based on order Id
        /// </summary>
        /// <param name="orderId">The id of the order to display</param>
        public void DisplayOrderDetails(int orderId)
        {
            var joinResults = context.OrderedProducts.Join(
                context.Products,
                order => order.ProductId,
                prod => prod.ProductId,
                (order, prod) => new
                {
                    OrderId = order.OrderId,
                    ProductName = prod.ProductName,
                    NumOrdered = order.NumberOrdered,
                    ProductPrice = prod.Price,
                }

            );
            var searchResult = joinResults.Where(x => x.OrderId == orderId);
            var orderResult = context.Orders.Where(x => x.OrderId == orderId).FirstOrDefault();
            var locationResult = context.Locations.Where(x => x.LocationId == orderResult.LocationId).FirstOrDefault();
            var customerResult = context.Customers.Where(x => x.CustomerId == orderResult.CustomerId).FirstOrDefault();


            Console.Clear();
            Console.WriteLine($"\n\t\t{"Order Id:",-20} {orderId}");
            Console.WriteLine($"\t\t{"Location Name:",-20} {locationResult.LocationName}");
            Console.WriteLine($"\t\t{"Customer Name:",-20} {customerResult.FirstName} {customerResult.LastName}");
            Console.WriteLine($"\t\t{"Order Time:", -20} {orderResult.OrderTime}\n");
            Console.WriteLine($"\t{"Product Name", -16} {"Num Ordered", -12} {"Price", -12}\n");
            decimal sum = 0;
            foreach (var obj in searchResult)
            {
                Console.WriteLine($"\t{obj.ProductName,-16} {obj.NumOrdered,-12} {(obj.ProductPrice * obj.NumOrdered),-12}");
                sum += (obj.ProductPrice * obj.NumOrdered);
            }
            Console.Write($"\n\t\tTotal Price: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"${sum}\n\n\n");
            Console.ForegroundColor = ConsoleColor.White;



            Console.WriteLine("Returning to main menu");
            Console.WriteLine("Press Enter to continue... ");
            Console.ReadLine();
            Startup();
        }


    }// end class
}// end namespace

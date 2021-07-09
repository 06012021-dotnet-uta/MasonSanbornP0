using CurrentUserNamespace;
using P0DbContext.Models;



namespace P0Main
{
    class main
    {
        static void Main(string[] args)
        {
            // Load Database
            P0DbClass context = new P0DbClass();


            // Create object for current user
            CurrentUser user = new CurrentUser();

            // Create new view controller
            BuissnessModel currentProgram = new BuissnessModel(user, context);


            do
            {
                Login:
                // Program info / user login / user register
                currentProgram.Startup();

                currentProgram.ChooseLocation();

                //currentProgram.RunProgram();
                
                while (true)
                {
                    int displayOptionsChoice = currentProgram.MainMenuOptions();
                    switch (displayOptionsChoice)
                    {
                        case 1: // Shop
                            currentProgram.ShopAtLocation();
                            continue;
                        case 2: // Change Locations
                            currentProgram.ChooseLocation();
                            break;
                        case 3: // logout
                            // do logout handling?
                            goto Login;

                    }

                }


            } while (true);


        }
    }
}

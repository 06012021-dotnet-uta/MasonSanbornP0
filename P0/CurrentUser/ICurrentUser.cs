using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P0DbContext.Models;

namespace CurrentUserNamespace
{
    interface ICurrentUser
    {
        public Customer currentCustomer { get; set; }
        public Location currentLocation { get; set; }

        public int getUserInputInt(int minVal, int maxVal);
        public bool getUserInputYN(string questionMessage);
        public string getUserInputString(string displayMessage);
    }
}

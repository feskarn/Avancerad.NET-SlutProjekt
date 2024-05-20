using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class User
    {
        // Unikt identifierare för användaren
        public int Id { get; set; }

        // Användarnamn, ofta används för inloggning
        public string Username { get; set; }

        public string PasswordHash { get; set; }

        // Roll som användaren tillhör, styr åtkomsträttigheter
        public string Role { get; set; }

        public bool IsActive { get; set; }

    }
}


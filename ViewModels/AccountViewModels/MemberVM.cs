using System.Collections.Generic;
using Wolmart.Ecommerce.Models;

namespace Wolmart.Ecommerce.ViewModels.AccountViewModels
{
    public class MemberVM
    {
        public ProfileVM ProfileVM { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMWeb.Models.ViewModel
{
    public class ContactVM
    {
        public IEnumerable<SelectListItem> AccountList { get; set; }
        public Contact Contact { get; set; }
    }
}

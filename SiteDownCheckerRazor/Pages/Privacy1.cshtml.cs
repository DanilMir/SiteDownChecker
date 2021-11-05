using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteDownCheckerRazor.Pages
{
    public class PrivacyModel1 : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel1(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}

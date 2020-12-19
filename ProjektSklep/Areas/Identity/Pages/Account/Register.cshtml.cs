using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using ProjektSklep.Data;
using ProjektSklep.Models;

namespace ProjektSklep.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Customer> _signInManager;
        private readonly UserManager<Customer> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<Customer> userManager,
            SignInManager<Customer> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            // Dane do Adresu
            [Required]
            public string Country { get; set; }
            [Required]
            public string Town { get; set; }
            [Required]
            public string PostCode { get; set; }
            [Required]
            public string Street { get; set; }
            [Required]
            public int HouseNumber { get; set; }
            public int? ApartmentNumber { get; set; }

            //imie i nazwisko do tabeli Customer 
            [Required]
            public string FirstName { get; set; }
            [Required]
            public string LastName { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new Customer { UserName = Input.Email, Email = Input.Email, FirstName = Input.FirstName, LastName = Input.LastName };

                Address address = null;
                PageConfiguration pageConfiguration = null;
                using (var context = new ShopContext())
                {
                    address = new Address { CustomerID = user.Id, Country = Input.Country, Town = Input.Town, PostCode = Input.PostCode, Street = Input.Street, HouseNumber = Input.HouseNumber, ApartmentNumber = Input.ApartmentNumber };
                    pageConfiguration = new PageConfiguration { CustomerID = user.Id, SendingNewsletter = false, ShowNetPrices = true, ProductsPerPage = 20, InterfaceSkin = 0, Language = 0, Currency = 1 };
                    context.Addresses.Add(address);
                    context.PageConfigurations.Add(pageConfiguration);
                    context.SaveChanges();
                }

                user.AddressID = address.AddressID;
                user.PageConfigurationID = pageConfiguration.PageConfigurationID;

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "NormalUser").Wait();

                    ////////// dodanie Customera do naszej bazy ////////// 
                    /*using (var context = new ShopContext())
                    {
                        var address = new Address
                        {
                            Country = Input.Country,
                            Town = Input.Town,
                            PostCode = Input.PostCode,
                            Street = Input.Street,
                            HouseNumber = Input.HouseNumber,
                            ApartmentNumber = Input.ApartmentNumber
                        };
                        context.Addresses.Add(address);
                        context.SaveChanges();
                        var pageConfiguration = new PageConfiguration
                        {
                            SendingNewsletter=false, 
                            ShowNetPrices=true, 
                            ProductsPerPage=20, 
                            InterfaceSkin=0, 
                            Language=0, 
                            Currency=1,
                        };
                        context.PageConfigurations.Add(pageConfiguration);
                        context.SaveChanges();
                        var customer = new Customer
                        {
                            Email = Input.Email,
                            FirstName = Input.FirstName,
                            LastName = Input.LastName,
                            AddressID = address.AddressID,
                            PageConfigurationID = pageConfiguration.PageConfigurationID
                        };
                        context.Customers.Add(customer);
                        context.SaveChanges();

                        var pageConfiguration2 = context.PageConfigurations.Where(x => x.PageConfigurationID == pageConfiguration.PageConfigurationID).FirstOrDefault();
                        if (pageConfiguration2 != null)
                        {
                            pageConfiguration2.CustomerID = customer.Id;
                        }
                        var address2 = context.Addresses.Where(x => x.AddressID == address.AddressID).FirstOrDefault();
                        if (address2 != null)
                        {
                            address2.CustomerID = customer.Id;
                        }
                        context.SaveChanges();
                    }*/
                    ////////// dodanie Customera do naszej bazy ////////// 

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}

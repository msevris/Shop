using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Shop.Application.Cart;
using Stripe;

namespace Shop.UI.Pages.Checkout
{
    public class PaymentModel : PageModel
    {
        public PaymentModel(IConfiguration config )
        {
            PublicKey = config["Stripe:PublicKey"].ToString();
        }

        public string PublicKey { get; }

        public IActionResult OnGet()
        {
            var information = new GetCustomerInformation(HttpContext.Session).Do();

            if (information == null)
            {
                return RedirectToPage("/Checkout/CustomerInformation");
            }
            return Page();
        }

        public IActionResult OnPost(string stripeEmail)
        {
            // Set your secret key: remember to change this to your live secret key in production
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            StripeConfiguration.ApiKey = "sk_test_epfhwDGKqA2eerOjsogN1lf900Yqj21wNr";

            // Create a Customer:
            var customerOptions = new CustomerCreateOptions
            {
                Email = stripeEmail,
            };
            var customerService = new CustomerService();
            Customer customer = customerService.Create(customerOptions);

            // Charge the Customer instead of the card:
            var chargeOptions = new ChargeCreateOptions
            {
                Amount = 1000,
                Currency = "usd",
                Customer = customer.Id,
            };
            var chargeService = new ChargeService();
            Charge charge = chargeService.Create(chargeOptions);

            // YOUR CODE: Save the customer ID and other info in a database for later.

            // When it's time to charge the customer again, retrieve the customer ID.
            var options = new ChargeCreateOptions
            {
                Amount = 1500, // $15.00 this time
                Currency = "usd",
                Customer = customer.Id, // Previously stored, then retrieved
            };
            


            return RedirectToPage("/Index");
        }
    }
}


// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using aracKiralamaDeneme.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace aracKiralamaDeneme.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<PersonalDataModel> _logger;
        private readonly CarRentalContext _context;

        public PersonalDataModel(
            UserManager<ApplicationUser> userManager,
            ILogger<PersonalDataModel> logger,
            CarRentalContext context)
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }

        public Customer Customer { get; set; }
        public List<RentalDetails> Rentals { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            // Customer'ı ve ilişkili Rentals ve RentalDetails'i çek
            var customer = await _context.Customers
                .Include(c => c.Addresses)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);

            if (customer != null)
            {
                Customer = customer;

                // Müşterinin kiraladığı araçları ve araç bilgilerini yükle
                Rentals = await _context.RentalDetails
                    .Include(rd => rd.Rental)
                        .ThenInclude(r => r.Vehicle)  // Kiralanan araç bilgisi
                    .Where(rd => rd.Rental.CustomerId == customer.CustomerId)
                    .ToListAsync();
            }

            return Page();
        }

        // Adres ekleme
        [BindProperty]
        public string NewAddressLine1 { get; set; }
        [BindProperty]
        public string? NewAddressLine2 { get; set; }
        [BindProperty]
        public string NewCity { get; set; }
        [BindProperty]
        public string NewZipCode { get; set; }
        [BindProperty]
        public string NewCountry { get; set; }

        public async Task<IActionResult> OnPostAddAddressAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var customer = await _context.Customers
                .Include(c => c.Addresses)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);
            if (customer == null) return NotFound();

            var address = new Address
            {
                CustomerId = customer.CustomerId,
                AddressLine1 = NewAddressLine1,
                AddressLine2 = NewAddressLine2,
                City = NewCity,
                ZipCode = NewZipCode,
                Country = NewCountry
            };

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            // **Redirect kullanarak OnGetAsync tekrar çalışsın**
            return RedirectToPage();
        }

        // Adres silme
        public async Task<IActionResult> OnPostDeleteAddressAsync(int addressId)
        {
            var address = await _context.Addresses.FindAsync(addressId);
            if (address != null)
            {
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCancelRentalAsync(int id)
        {
            var rental = await _context.Rentals
                .Include(r => r.Vehicle)
                .Include(r => r.RentalDetails)
                .FirstOrDefaultAsync(r => r.RentalId == id);

            if (rental == null)
            {
                TempData["Error"] = "Kiralama bulunamadı.";
                return RedirectToPage(); // Aynı sayfada kal
            }

            foreach (var detail in rental.RentalDetails)
                detail.Status = "İptal";

            rental.Vehicle.Status = "Müsait";

            _context.Update(rental);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Kiralama iptal edildi.";
            return RedirectToPage(); // Aynı sayfada kal
        }

    }
}

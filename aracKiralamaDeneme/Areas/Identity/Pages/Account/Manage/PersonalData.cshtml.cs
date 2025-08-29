// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using aracKiralamaDeneme.Models;
using aracKiralamaDeneme.Models.DTO;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Threading.Tasks;

namespace aracKiralamaDeneme.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<PersonalDataModel> _logger;
        private readonly IDbConnection _connection;

        public PersonalDataModel(
            UserManager<ApplicationUser> userManager,
            ILogger<PersonalDataModel> logger,
            IDbConnection connection)
        {
            _userManager = userManager;
            _logger = logger;
            _connection = connection;
        }

        public Customer Customer { get; set; }
        //public List<RentalDetails> Rentals { get; set; }
        public List<RentalWithDetailsDto> Rentals { get; set; } = new();


        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            // Customer ve Address'leri SP ile çek
            Customer = await _connection.QueryFirstOrDefaultAsync<Customer>(
                "GetCustomerByUserId",
                new { UserId = user.Id },
                commandType: CommandType.StoredProcedure
            );

            if (Customer != null)
            {
                // Adresleri yükle
                Customer.Addresses = (await _connection.QueryAsync<Address>(
                    "GetAddressesByCustomerId",
                    new { CustomerId = Customer.CustomerId },
                    commandType: CommandType.StoredProcedure
                )).ToList();

                // Kiralamaları DTO ile yükle
                Rentals = (await _connection.QueryAsync<RentalWithDetailsDto>(
                    "GetCustomerRentals",
                    new { CustomerId = Customer.CustomerId },
                    commandType: CommandType.StoredProcedure
                )).ToList();
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

        // OnPostAddAddressAsync
        public async Task<IActionResult> OnPostAddAddressAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var customer = await _connection.QueryFirstOrDefaultAsync<Customer>(
                "GetCustomerByUserId",
                new { UserId = user.Id },
                commandType: CommandType.StoredProcedure);

            if (customer == null) return NotFound();

            await _connection.ExecuteAsync(
                "AddAddress", // eski AddAddress yerine yeni SP
                new
                {
                    CustomerId = customer.CustomerId,
                    AddressLine1 = NewAddressLine1,
                    AddressLine2 = NewAddressLine2,
                    City = NewCity,
                    ZipCode = NewZipCode,
                    Country = NewCountry
                },
                commandType: CommandType.StoredProcedure);

            return RedirectToPage();
        }


        // Adres Silme
        public async Task<IActionResult> OnPostDeleteAddressAsync(int addressId)
        {
            await _connection.ExecuteAsync(
                "DeleteAddress",
                new { AddressId = addressId },
                commandType: CommandType.StoredProcedure);

            return RedirectToPage();
        }


        // OnPostCancelRentalAsync
        public async Task<IActionResult> OnPostCancelRentalAsync(int id)
        {
            // RentalDetails ve Vehicle durumlarını güncelle
            await _connection.ExecuteAsync(
                "CancelRental",
                new { RentalId = id },
                commandType: CommandType.StoredProcedure);

            TempData["Success"] = "Kiralama iptal edildi.";
            return RedirectToPage();
        }
    }
}

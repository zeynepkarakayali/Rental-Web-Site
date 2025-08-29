// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using aracKiralamaDeneme.Models;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace aracKiralamaDeneme.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IDbConnection _connection;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IDbConnection connection)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _connection = connection;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            /// [Display(Name = "First Name")]
            /// 
            [Display(Name = "Ad")]
            public string FirstName { get; set; }

            [Display(Name = "Soyad")]
            public string LastName { get; set; }

            [Display(Name = "Ehliyet Numarası")]
            public string LicenseNumber { get; set; }

            [Display(Name = "Ehliyet Türü")]
            public string LicenseType { get; set; }

            [Phone]
            [Display(Name = "Telefon Numarası")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            var customer = await _connection.QueryFirstOrDefaultAsync<Customer>(
                "GetCustomerByUserId",
                new { UserId = user.Id },
                commandType: CommandType.StoredProcedure);

            Username = userName;

            Input = new InputModel
            {
                FirstName = customer?.FirstName,
                LastName = customer?.LastName,
                LicenseNumber = customer?.LicenseNumber,
                LicenseType = customer?.LicenseType,
                PhoneNumber = customer?.PhoneNumber
            };
        }


        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Telefon numarası güncellenemedi.";
                    return RedirectToPage();
                }
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            // SP ile güncelle
            await _connection.ExecuteAsync(
                "SP_UpdateCustomerPhone",
                new { UserId = user.Id, PhoneNumber = Input.PhoneNumber },
                commandType: CommandType.StoredProcedure);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Profiliniz güncellendi.";
            return RedirectToPage();
        }

    }
}

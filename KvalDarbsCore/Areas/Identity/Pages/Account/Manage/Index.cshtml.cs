using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LogicCore;
using LogicCore.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Task = System.Threading.Tasks.Task;

namespace KvalDarbsCore.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [RequiredLocalized]
            [Display(Name = "Username", ResourceType = typeof(Text))]
            public string Username { get; set; }

            [RequiredLocalized]
            [Display(Name = "Name", ResourceType = typeof(Text))]
            public string Name { get; set; }

            [RequiredLocalized]
            [Display(Name = "Surname", ResourceType = typeof(Text))]
            public string Surname { get; set; }

            [Phone]
            [Display(Name = "PhoneNumber", ResourceType = typeof(Text))]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = user.UserName;
            var phoneNumber = user.PhoneNumber;
            var name = user.Name;
            var surname = user.Surname;

            Input = new InputModel
            {
                Username = userName,
                Name = name,
                Surname = surname,
                PhoneNumber = phoneNumber
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

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            bool changes = false;
            
            if (user.Name != Input.Name)
            {
                changes = true;
                user.Name = Input.Name;
            }

            if (user.Surname != Input.Surname)
            {
                changes = true;
                user.Surname = Input.Surname;
            }

            if (user.UserName != Input.Username)
            {
                changes = true;
                user.UserName = Input.Username;
            }

            if (user.PhoneNumber != Input.PhoneNumber)
            {
                changes = true;
                user.PhoneNumber = Input.PhoneNumber;
            }

            if (changes)
            {
                var updateUser = await _userManager.UpdateAsync(user);

                if (!updateUser.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }

                await _signInManager.RefreshSignInAsync(user);
                StatusMessage = "Your profile has been updated";
            }
            return RedirectToPage();
        }
    }
}

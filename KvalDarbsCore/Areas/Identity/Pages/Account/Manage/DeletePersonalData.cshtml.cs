using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KvalDarbsCore.Data;
using LogicCore;
using LogicCore.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KvalDarbsCore.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<DeletePersonalDataModel> _logger;
        private readonly ApplicationDbContext _context;

        public DeletePersonalDataModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<DeletePersonalDataModel> logger,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [RequiredLocalized]
            [DataType(DataType.Password)]
            [Display(Name = "Password", ResourceType = typeof(Text))]
            public string Password { get; set; }
        }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, ErrorText.PasswordMismatch);
                    return Page();
                }
            }

            var userId = await _userManager.GetUserIdAsync(user);

            try
            {
                _context.Database.BeginTransaction();
                var competitions = _context.Competitions.Where(m => m.AuthorId == user.Id).ToList();
                var exercises = _context.Exercises.Where(m => m.AuthorId == user.Id).ToList();
                var teams = _context.Teams.Include(m => m.Members).ThenInclude(m => m.User).Include(m => m.Notifications).Where(m => m.Coach.Id == user.Id).ToList();
                var deletableTeams = new List<Team>();
                var editedTeams = new List<Team>();
                var trainings = _context.Trainings.Where(m => m.UserId == user.Id).ToList();
                var notifications = _context.Notifications.Where(m => m.UserId == user.Id).ToList();
                var userTeams = _context.UserTeams.Where(m => m.UserId == user.Id).ToList();
                var results = _context.Results.Where(m => m.AthleteId == user.Id).ToList();
                competitions.ForEach(m => { m.Author = null; m.AuthorId = null; });
                exercises.ForEach(m => { m.Author = null; m.AuthorId = null; });
                teams.ForEach(m => {
                    if (!m.Members.Any(t => t.UserId != user.Id))
                    {
                        deletableTeams.Add(m);
                    }
                    else
                    {
                        m.Coach = m.Members.First(t => t.UserId != user.Id).User;
                        editedTeams.Add(m);
                    }
                });
                deletableTeams.ForEach(m => {
                    notifications.AddRange(_context.Notifications.Where(t => t.TeamId == m.Id).ToList());
                });

                _context.UpdateRange(competitions);
                _context.UpdateRange(exercises);
                _context.UpdateRange(editedTeams);
                _context.RemoveRange(userTeams);
                _context.RemoveRange(results);
                _context.RemoveRange(trainings);
                _context.RemoveRange(notifications);
                _context.RemoveRange(deletableTeams);
                _context.Remove(user);
                _context.SaveChanges();
                _context.Database.CommitTransaction();
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                _logger.LogInformation(ex.Message);
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/");
        }
    }
}

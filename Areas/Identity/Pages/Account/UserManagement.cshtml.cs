using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RepairTracker.Areas.Identity.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class UserManagementModel : PageModel
{
    private readonly UserManager<RepairTrackerUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserManagementModel(UserManager<RepairTrackerUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public List<RepairTrackerUser> Users { get; set; } = new();
    public List<IdentityRole> AllRoles { get; set; } = new();

    public async Task OnGetAsync()
    {
        Users = _userManager.Users.ToList();
        AllRoles = _roleManager.Roles.ToList();
    }

    public async Task<IActionResult> OnPostAddToRoleAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null && await _roleManager.RoleExistsAsync(roleName))
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRemoveFromRoleAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null && await _roleManager.RoleExistsAsync(roleName))
        {
            await _userManager.RemoveFromRoleAsync(user, roleName);
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateRolesAsync(string userId, List<string> selectedRoles)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var currentRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

            // Add new roles
            var rolesToAdd = selectedRoles.Except(currentRoles).ToList();
            if (rolesToAdd.Any())
                await _userManager.AddToRolesAsync(user, rolesToAdd);

            // Remove unchecked roles
            var rolesToRemove = allRoles.Except(selectedRoles).Intersect(currentRoles).ToList();
            if (rolesToRemove.Any())
                await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
        }
        return RedirectToPage();
    }
}

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BlazorUserRoles.Pages;

public class Administration : ComponentBase
{
    [CascadingParameter]
    private Task<AuthenticationState>? authenticationStateTask { get; set; }

    protected UserManager<IdentityUser>? _UserManager;
    protected RoleManager<IdentityRole>? _RoleManager;
    
    protected string ADMINISTRATION_ROLE = "Administrators";
    ClaimsPrincipal? CurrentUser;

    protected override async Task OnInitializedAsync()
    {
        //Ensure there is an ADMINSTRATION_ROLE
        var RoleResult = await _RoleManager.FindByNameAsync(ADMINISTRATION_ROLE);

        if (RoleResult == null)
        {
            //Create the ADMINISTRATION_ROLE
            await _RoleManager.CreateAsync(new IdentityRole(ADMINISTRATION_ROLE));
        }

        //Ensure a user named Admin@BlazorHelpWebsite.com is an Administrator
        var user = await _UserManager.FindByNameAsync("Admin@BlazorHelpWebsite.com");

        if (user != null)
        {
            // Is Admin@BlazorHelpWebsite.com an administrative role?
            var UserResult = await _UserManager.IsInRoleAsync(user, ADMINISTRATION_ROLE);

            if (!UserResult)
            {
                //Put admin in the Administrator role
                await _UserManager.AddToRoleAsync(user, ADMINISTRATION_ROLE);
            }
        }

        // Get the current logged in user
        CurrentUser = (await authenticationStateTask).User;
    }
}
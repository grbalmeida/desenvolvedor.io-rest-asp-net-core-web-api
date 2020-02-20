using DevIO.Api.ViewModels;
using DevIO.Business.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DevIO.Api.Controllers
{
    [Route("api/account")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(INotifier notifier,
                              SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager): base(notifier)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("new-account")]
        public async Task<ActionResult> RegisterUser(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return CustomResponse(registerUser);
            }

            foreach (var error in result.Errors)
            {
                NotifyError(error.Description);
            }

            return CustomResponse(registerUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(
                loginUser.Email,
                loginUser.Password,
                isPersistent: false,
                lockoutOnFailure: true // Block the user if he enters invalid data multiple times
            );

            if (result.Succeeded)
            {
                return CustomResponse(loginUser);
            }

            if (result.IsLockedOut)
            {
                NotifyError("User temporarily blocked by invalid attempts");
                return CustomResponse(loginUser);
            }

            NotifyError("Incorrect username or password");
            return CustomResponse(loginUser);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Service.Commons;

namespace GestionAntioquia.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSendGrid _emailSendGrid;

        public RegisterConfirmationModel(UserManager<IdentityUser> userManager, IEmailSendGrid emailSendGrid)
        {
            _userManager = userManager;
            _emailSendGrid = emailSendGrid;
        }

        public string Email { get; set; }

        public bool DisplayConfirmAccountLink { get; set; }

        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"No se puede cargar al usuario con el correo electrónico '{email}'.");
            }

            Email = email;
            // Una vez que agregue un remitente de correo electrónico real, debe eliminar este código que le permite confirmar la cuenta
            //DisplayConfirmAccountLink = true;
            //if (DisplayConfirmAccountLink)
            //{
            //    var userId = await _userManager.GetUserIdAsync(user);
            //    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            //    EmailConfirmationUrl = Url.Page(
            //        "/Account/ConfirmEmail",
            //        pageHandler: null,
            //        values: new { area = "Identity", userId = userId, code = code },
            //        protocol: Request.Scheme);
            //}


            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            string _body = $"Por favor haga clic en el siguiente enlace para activar la cuenta <a href='{code}'>Inicio</a>";

            await _emailSendGrid.Execute("Confirmación de cuenta", _body, user.Email);

            return Page();
        }
    }
}

using System.Text;
using Application.Interfaces;
using Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Application.DTOs;

namespace Identity.Services
{
    public class AccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;

        public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public async Task<LoginResponseDto> AuthenticateAsync(LoginDto loginDto)
        {
            LoginResponseDto response = new() { Email = "", Id = "", LastName = "", Name = "", UserName = "", HasError = false, Errors = [] };

            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null)
            {
                response.HasError = true;
                response.Errors.Add($"Email o contraseña incorrecta");
                return response;
            }

            if (!user.EmailConfirmed)
            {
                response.HasError = true;
                response.Errors.Add("Su cuenta aun no esta activada, deberias revisar tu email");
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName ?? "", loginDto.Password, false, true);

            if (!result.Succeeded)
            {
                response.HasError = true;
                if (result.IsLockedOut)
                {
                    response.Errors.Add($"Su cuenta ha sido bloqueada debido a multiples intentos de acceso fallidos." +
                        $" Intente nuevamente en 10 minutos.");
                }
                else
                {
                    response.Errors.Add($"Email o contraseña incorrecta");
                }
                return response;
            }

            var rolesList = await _userManager.GetRolesAsync(user);

            response.Id = user.Id;
            response.Email = user.Email ?? "";
            response.UserName = user.UserName ?? "";
            response.Name = user.Name;
            response.LastName = user.LastName;
            response.IsVerified = user.EmailConfirmed;
            response.Roles = rolesList.ToList();

            return response;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        /*
        public async Task<RegisterResponseDto> RegisterUser(RegisterUserDto registerUser, string origin)
        {
            RegisterResponseDto response = new()
            {
                Email = "",
                Id = "",
                LastName = "",
                Name = "",
                UserName = "",
                HasError = false,
                Errors = []
            };

            var userWithSameEmail = await _userManager.FindByNameAsync(registerUser.Email);
            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Errors.Add("Este email ya esta registrado");
                return response;
            }

            AppUser user = new AppUser()
            {
                Name = registerUser.Name,
                LastName = registerUser.LastName,
                Email = registerUser.Email,
                UserName = registerUser.Email,
                EmailConfirmed = false,
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, registerUser.Role);
                string verificationUri = await GetVerificationEmailUri(user, origin);
                await _emailService.SendAsync(new EmailRequestDto()
                {
                    To = registerUser.Email,
                    HtmlBody = $"Activa tu cuenta mediante este URL: {verificationUri}",
                    Subject = "Activa tu cuenta",
                });

                var rolesList = await _userManager.GetRolesAsync(user);

                response.Id = user.Id;
                response.Email = user.Email ?? "";
                response.UserName = user.UserName ?? "";
                response.Name = user.Name;
                response.LastName = user.LastName;
                response.IsVerified = user.EmailConfirmed;
                response.Roles = rolesList.ToList();

                return response;
            }
            else
            {
                response.HasError = true;
                response.Errors.AddRange(result.Errors.Select(s => s.Description).ToList());
                return response;
            }
        }*/

        public async Task<List<UserDto>> GetAllUser(bool? isActive = true)
        {
            // Fetch users first
            var query = _userManager.Users;

            if (isActive == true)
            {
                query = query.Where(w => w.EmailConfirmed);
            }

            var users = await query.ToListAsync();
            var listUsersDtos = new List<UserDto>();

            foreach (var item in users)
            {
                var roleList = await _userManager.GetRolesAsync(item);

                listUsersDtos.Add(new UserDto()
                {
                    Id = item.Id,
                    Email = item.Email ?? "",
                    LastName = item.LastName,
                    Name = item.Name,
                    UserName = item.UserName ?? "",
                    Phone = item.PhoneNumber,
                    IsVerified = item.EmailConfirmed,
                    Role = roleList.FirstOrDefault() ?? ""
                });
            }

            return listUsersDtos;
        }

        public async Task<UserDto?> GetUserByUserName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return null;
            }

            var rolesList = await _userManager.GetRolesAsync(user);

            var userDto = new UserDto()
            {
                Id = user.Id,
                Email = user.Email ?? "",
                LastName = user.LastName,
                Name = user.Name,
                UserName = user.UserName ?? "",
                Phone = user.PhoneNumber,
                IsVerified = user.EmailConfirmed,
                Role = rolesList.FirstOrDefault() ?? ""
            };

            return userDto;
        }


        public async Task<UserDto?> GetUserById(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);

            if (user == null)
            {
                return null;
            }

            var rolesList = await _userManager.GetRolesAsync(user);

            var userDto = new UserDto()
            {
                Id = user.Id,
                Email = user.Email ?? "",
                LastName = user.LastName,
                Name = user.Name,
                UserName = user.UserName ?? "",
                Phone = user.PhoneNumber,
                IsVerified = user.EmailConfirmed,
                Role = rolesList.FirstOrDefault() ?? ""
            };

            return userDto;
        }


        public async Task<UserResponseDto> DeleteAsync(string id)
        {
            UserResponseDto response = new() { HasError = false, Errors = [] };
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                response.HasError = true;
                response.Errors.Add($"No hay cuenta registrada con este Id");
                return response;
            }

            await _userManager.DeleteAsync(user);

            return response;
        }

        public async Task<string> ConfirmAccountAsync(string userId, string token)
        {

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return "Email o contraseña incorrecta";
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return $"Cuenta {user.Email} confirmada. Ahora puedes acceder al sistema";
            }
            else
            {
                return $"Un error ha ocurrido confirmando este email {user.Email}";
            }
        }

        /*
        public async Task<EditResponseDto> EditUser(RegisterUserDto registerUser, string origin, bool? isCreated = false)
        {
            bool isNotcreated = !isCreated ?? false;
            EditResponseDto response = new()
            {
                Email = "",
                Id = "",
                LastName = "",
                Name = "",
                UserName = "",
                HasError = false,
                Errors = []
            };

            var userWithSameUserName = await _userManager.Users.FirstOrDefaultAsync(w => w.UserName == registerUser.Email && w.Id != registerUser.Id);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Errors.Add($"El email {registerUser.Email} ya esta en el sistema");
                return response;
            }

            var user = await _userManager.FindByIdAsync(registerUser.Id);

            if (user == null)
            {
                response.HasError = true;
                response.Errors.Add($"No hay cuenta registrada con este Id");
                return response;
            }

            user.Name = registerUser.Name;
            user.LastName = registerUser.LastName;
            user.UserName = registerUser.Email;
            user.EmailConfirmed = user.EmailConfirmed && user.Email == registerUser.Email;
            user.Email = registerUser.Email;
            user.PhoneNumber = registerUser.Phone;

            if (!string.IsNullOrWhiteSpace(registerUser.Password) && isNotcreated)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resultChange = await _userManager.ResetPasswordAsync(user, token, registerUser.Password);

                if (resultChange != null && !resultChange.Succeeded)
                {
                    response.HasError = true;
                    response.Errors.AddRange(resultChange.Errors.Select(s => s.Description).ToList());
                    return response;
                }
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var rolesList = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, rolesList.ToList());

                await _userManager.AddToRoleAsync(user, registerUser.Role);


                if (!user.EmailConfirmed && isNotcreated)
                {
                    string verificationUri = await GetVerificationEmailUri(user, origin);
                    await _emailService.SendAsync(new EmailRequestDto()
                    {
                        To = registerUser.Email,
                        HtmlBody = $"Activa tu cuenta mediante este URL: {verificationUri}",
                        Subject = "Activa tu cuenta"
                    });
                }

                var updatedRolesList = await _userManager.GetRolesAsync(user);

                response.Id = user.Id;
                response.Email = user.Email ?? "";
                response.UserName = user.UserName ?? "";
                response.Name = user.Name;
                response.LastName = user.LastName;
                response.IsVerified = user.EmailConfirmed;
                response.Roles = updatedRolesList.ToList();

                return response;
            }
            else
            {
                response.HasError = true;
                response.Errors.AddRange(result.Errors.Select(s => s.Description).ToList());
                return response;
            }
        }*/


        #region Private methods
        /*private async Task<string> GetVerificationEmailUri(AppUser user, string origin)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var route = "Login/ConfirmEmail";
            var completeUrl = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(completeUrl.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri.ToString(), "token", token);
            return verificationUri;
        }*/
        #endregion
    }
}


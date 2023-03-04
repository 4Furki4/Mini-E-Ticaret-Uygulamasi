using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.DTOs.Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Domain.Entities.Identity.AppUsers;
using Microsoft.AspNetCore.Identity;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Application.Features.Commands.AppUserCommands;
using Org.BouncyCastle.Asn1.Ocsp;

namespace ETicaretAPI.Persistence.Services
{
    public class AuthService : IAuthService
    {
        readonly HttpClient httpClient;
        readonly IConfiguration configuration;
        readonly UserManager<AppUser> userManager;
        readonly ITokenHandler tokenHandler;
        readonly SignInManager<AppUser> signInManager;
        public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration, UserManager<AppUser> userManager, ITokenHandler tokenHandler, SignInManager<AppUser> signInManager)
        {
            httpClient = httpClientFactory.CreateClient();
            this.configuration = configuration;
            this.userManager = userManager;
            this.tokenHandler = tokenHandler;
            this.signInManager = signInManager;
        }

        private async Task ConfirmOrCreateUser(UserLoginInfo userInfo ,string email, string name)
        {
            AppUser? user = await userManager.FindByLoginAsync(userInfo.LoginProvider, userInfo.ProviderKey);


            bool result = user != null;
            if (user is null)
            {
                user = await userManager.FindByEmailAsync(email);
                if (user is null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = email,
                        UserName = email,
                        FullName = name
                    };
                    IdentityResult identityResult = await userManager.CreateAsync(user);
                    result = identityResult.Succeeded;
                }
            }
            if (result)
                await userManager.AddLoginAsync(user, userInfo);
            else
                throw new Exception("Invalid external login!");
        }
        public async Task<Token> FacebookLoginAsync(string authToken, int tokenLifeTime)
        {
            string accessToken = await httpClient
                .GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={configuration["ExternalLoginSettings:Facebook:Client_Id"]}&client_secret={configuration["ExternalLoginSettings:Facebook:Client_Secret"]}&grant_type=client_credentials");

            FacebookAccessTokenResponse? facebookAccessTokenResponse = JsonSerializer.Deserialize<FacebookAccessTokenResponse>(accessToken);
            string userAccessTokenValidation = await httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={authToken}&access_token={facebookAccessTokenResponse?.AccessToken}");
            FacebookAccessValidationData? Validation = JsonSerializer.Deserialize<FacebookAccessValidationData>(userAccessTokenValidation);
            if (Validation?.Data.IsValid != null)
            {
                string UserInfo = await httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={authToken}");
                JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                FacebookUserInfo? facebookUserInfo = JsonSerializer.Deserialize<FacebookUserInfo>(UserInfo, options);

                UserLoginInfo userLoginInfo = new("FACEBOOK", Validation.Data.UserId, "FACEBOOK");

                AppUser? user = await userManager.FindByLoginAsync(userLoginInfo.LoginProvider, userLoginInfo.ProviderKey);
                await ConfirmOrCreateUser(userLoginInfo, facebookUserInfo?.Email, facebookUserInfo?.Name);
                Token token = tokenHandler.CreateAccessToken(tokenLifeTime);
                return token;
            }
            else
            {
                throw new Exception("Invalid external login!");
            }

        }

        public async Task<Token> GoogleLoginAsync(string idToken, int tokenLifeTime)
        {
            var settings = new ValidationSettings()
            {
                Audience = new List<string> { configuration["ExternalLoginSettings:Google:Client_Id"] }
            };

            Payload payload = await ValidateAsync(idToken, settings);
            UserLoginInfo userInfo = new("GOOGLE", payload.Subject, "GOOGLE");

            await ConfirmOrCreateUser(userInfo, payload.Email, payload.GivenName);

            Token token = tokenHandler.CreateAccessToken(tokenLifeTime);
            return token;
        }

        public async Task<Token> LoginAsync(string usernameOrEmail, string password)
        {
            AppUser? appUser = await userManager.FindByNameAsync(usernameOrEmail);

            if (appUser is null)
                appUser = await userManager.FindByEmailAsync(usernameOrEmail);
            if (appUser is null)
            {
                throw new LoginFailedException();
            }

            SignInResult result = await signInManager.CheckPasswordSignInAsync(appUser, password, false);
            if (result.Succeeded)
            {
                Token token = tokenHandler.CreateAccessToken(5);
                return token;
            }
            else
                throw new LoginFailedException();
        }
    }
}

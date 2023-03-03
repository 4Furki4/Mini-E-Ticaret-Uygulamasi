using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.DTOs.Facebook;
using ETicaretAPI.Application.ViewModels.Identity;
using ETicaretAPI.Domain.Entities.Identity.AppUsers;
using Google.Apis.Auth.OAuth2;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUserCommands
{
    public class FacebookLoginCommand : IRequest<FacebookLoginCommandResponse>
    {
        public FacebookLoginViewModel ViewModel { get; set; }

        public FacebookLoginCommand(FacebookLoginViewModel viewModel)
        {
            ViewModel = viewModel;
        }
    }

    public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommand, FacebookLoginCommandResponse>
    {
        readonly UserManager<AppUser> userManager;
        readonly ITokenHandler tokenHandler;
        readonly HttpClient httpClient;

        public FacebookLoginCommandHandler(UserManager<AppUser> userManager, ITokenHandler tokenHandler, IHttpClientFactory httpClientFactory)
        {
            this.userManager = userManager;
            this.tokenHandler = tokenHandler;
            httpClient = httpClientFactory.CreateClient();
        }

        public async Task<FacebookLoginCommandResponse> Handle(FacebookLoginCommand request, CancellationToken cancellationToken)
        {
            string accessToken = await httpClient
                .GetStringAsync("https://graph.facebook.com/oauth/access_token?client_id=748961220181075&client_secret=e9b1c022480b87de7c45841ecfc66f93&grant_type=client_credentials");
            
            FacebookAccessTokenResponse? facebookAccessTokenResponse = JsonSerializer.Deserialize<FacebookAccessTokenResponse>(accessToken);
            string userAccessTokenValidation = await httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={request.ViewModel.AuthToken}&access_token={facebookAccessTokenResponse?.AccessToken}");

            FacebookAccessValidationData? facebookAccessTokenValidationData = JsonSerializer.Deserialize<FacebookAccessValidationData>(userAccessTokenValidation);
            if (facebookAccessTokenValidationData.Data.IsValid)
            {
                string UserInfo = await httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={request.ViewModel.AuthToken}");
                JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                FacebookUserInfo? facebookUserInfo = JsonSerializer.Deserialize<FacebookUserInfo>(UserInfo, options);

                UserLoginInfo userLoginInfo = new("FACEBOOK", facebookAccessTokenValidationData.Data.UserId, "FACEBOOK");

                AppUser? user = await userManager.FindByLoginAsync(userLoginInfo.LoginProvider, userLoginInfo.ProviderKey);
                bool result = user != null;
                if (user is null)
                {
                    user = await userManager.FindByEmailAsync(facebookUserInfo.Email);

                    if(user is null)
                    {
                        user = new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Email = facebookUserInfo.Email,
                            UserName = facebookUserInfo.Email,
                            FullName = facebookUserInfo.Name
                        };
                        IdentityResult identityResult = await userManager.CreateAsync(user);
                        result = identityResult.Succeeded;
                    }
                }
                if (result)
                {
                    await userManager.AddLoginAsync(user, userLoginInfo);
                    Token token = tokenHandler.CreateAccessToken(15);
                    return new()
                    {
                        Token = token
                    };
                }
                else
                {
                    throw new Exception("Bu e-postaya sahip kullanıcı zaten mevcut!");
                }
            }
            throw new Exception("Invalid external login!");
        }
    }

    public class FacebookLoginCommandResponse
    {
        public Token Token { get; set; }
    }
}

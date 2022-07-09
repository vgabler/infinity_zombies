using Auth.Infra;
using Facebook.Unity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auth.External
{
    public class FacebookServiceImpl : IFacebookService
    {
        public FacebookServiceImpl()
        {
            FB.Init("1181205196052549");
        }

        public async Task<string> GetSignInToken()
        {
            var tcs = new TaskCompletionSource<ILoginResult>();

            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                var perms = new List<string>() { "public_profile", "email" };
                FB.LogInWithReadPermissions(perms, (result) => tcs.TrySetResult(result));
            });

            await tcs.Task;

            if (FB.IsLoggedIn)
            {
                // AccessToken class will have session details
                var aToken = AccessToken.CurrentAccessToken;
                // Print current access token's User ID
                UnityEngine.Debug.Log($"Facebook user id: {aToken.UserId}");
                // Print current access token's granted permissions
                foreach (string perm in aToken.Permissions)
                {
                    UnityEngine.Debug.Log($"Facebook permission granted: {perm}");
                }

                return aToken.TokenString;
            }
            else
            {
                UnityEngine.Debug.Log("User cancelled login");
                return null;
            }
        }
    }
}

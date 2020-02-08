using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace BlazorFirebaseAuthTemplate.Pages
{
    public class IndexBase: ComponentBase
    {
        public static string User { get; set; } = "Not Signed In";
        public static string Email { get; set; } = "Not Signed In";
        public static string Token { get; set; } = "--";

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [JSInvokable]
        public void LoginCallback(string email, string display, string token)
        {
            User = display;
            Email = email;
            Token = token;
            InvokeAsync(StateHasChanged);
        }


        [JSInvokable]
        public void LogoutCallback()
        {
            Console.WriteLine("Callback in C# called");
            User = "Not Signed In";
            Email = "Not Signed In";            
            InvokeAsync(StateHasChanged);
        }

        protected async Task Login()
        {
            await JSRuntime.InvokeAsync<object>("FirebaseLogin", DotNetObjectReference.Create(this));
        }

        protected async Task Logout()
        {
            await JSRuntime.InvokeAsync<object>("FirebaseLogout", DotNetObjectReference.Create(this));
        }



        //protected static async Task Login()
        //{
        //    var config = new FirebaseAuthConfig
        //    {
        //        ApiKey = "AIzaSyCE-VkSNlL5Gmba_7AW1drt9eKjZSnqBTc",
        //        AuthDomain = "btl-blazor.firebaseapp.com",
        //        Providers = new FirebaseAuthProvider[]
        //        {
        //            new GoogleProvider(),
        //            new FacebookProvider(),
        //            new TwitterProvider(),
        //            new GithubProvider(),
        //            new MicrosoftProvider(),
        //            new EmailProvider()
        //        }
        //    };

        //    if (config.ApiKey == "<YOUR API KEY>" || config.AuthDomain == "<YOUR PROJECT DOMAIN>.firebaseapp.com")
        //    {
        //        Console.WriteLine("You need to setup your API key and auth domain first in Program.cs");
        //        return;
        //    }

        //    var client = new FirebaseAuthClient(config);

        //    config.Providers.Select((provider, i) => (provider, i)).ToList().ForEach(p => Console.WriteLine($"[{p.i}]: {p.provider.ProviderType}"));

        //    var i = 0;
        //    UserCredential userCredential;

        //    if (i == config.Providers.Count())
        //    {
        //        userCredential = await client.SignInAnonymouslyAsync();
        //        var r = Console.ReadLine().ToLower();
        //        if (r == "e")
        //        {
        //            // link with email
        //            Console.Write("Enter email: ");
        //            var email = Console.ReadLine();
        //            Console.Write("Enter password: ");
        //            var password = ReadPassword();

        //            var credential = EmailProvider.GetCredential(email, password);
        //            userCredential = await userCredential.User.LinkWithCredentialAsync(credential);
        //        }
        //        else if (r == "r")
        //        {
        //            // link with redirect
        //            Console.WriteLine("How do you want to link?");
        //            config.Providers.Where(p => p.ProviderType != FirebaseProviderType.EmailAndPassword).Select((provider, i) => (provider, i)).ToList().ForEach(p => Console.WriteLine($"[{p.i}]: {p.provider.ProviderType}"));

        //            i = int.Parse(Console.ReadLine());
        //            userCredential = await userCredential.User.LinkWithRedirectAsync(config.Providers[i].ProviderType, uri =>
        //            {
        //                Console.WriteLine($"Go to \n{uri}\n and paste here the redirect uri after you finish signing in");
        //                return Task.FromResult(Console.ReadLine());
        //            });
        //        }

        //        if (r == "e" || r == "r")
        //        {
        //            // if linked, offer unlink
        //            Console.WriteLine("Unlink? [y/n]");
        //            if (Console.ReadLine().ToLower() == "y")
        //            {
        //                userCredential.User = await userCredential.User.UnlinkAsync(FirebaseProviderType.EmailAndPassword);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        var provider = config.Providers[i].ProviderType;
        //        userCredential = provider == FirebaseProviderType.EmailAndPassword
        //            ? await SignInWithEmail(client)
        //            : await client.SignInWithRedirectAsync(provider, uri =>
        //            {
        //                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

        //                Console.WriteLine($"Go to \n{uri}\n and paste here the redirect uri after you finish signing in");
        //                return Task.FromResult(Console.ReadLine());
        //            });

        //        Console.WriteLine($"You're signed in as {userCredential.User.Uid} | {userCredential.User.Info.DisplayName} | {userCredential.User.Info.Email}");
        //    }

        //    Console.WriteLine($"New password (empty to skip):");
        //    var pwd = ReadPassword();

        //    if (!string.IsNullOrWhiteSpace(pwd))
        //    {
        //        await userCredential.User.ChangePasswordAsync(pwd);
        //    }

        //    Console.WriteLine($"Trying to force refresh the idToken {userCredential.User.Credential.IdToken}");
        //    var token = await userCredential.User.GetIdTokenAsync(true);
        //    Console.WriteLine($"Success, new token: {token}");

        //    Console.WriteLine("Delete this account? [y/n]");
        //    if (Console.ReadLine().ToLower() == "y")
        //    {
        //        await userCredential.User.DeleteAsync();
        //    }
        //}

        //private static async Task<UserCredential> SignInWithEmail(FirebaseAuthClient client)
        //{
        //    Console.Write("Enter email: ");
        //    var email = Console.ReadLine();

        //    try
        //    {
        //        var result = await client.FetchSignInMethodsForEmailAsync(email);

        //        if (result.UserExists && result.AllProviders.Contains(FirebaseProviderType.EmailAndPassword))
        //        {
        //            Console.Write("User exists, enter password: ");
        //            var password = ReadPassword();
        //            var emailUser = await client.SignInWithEmailAndPasswordAsync(email, password);

        //            return emailUser;
        //        }
        //        else
        //        {
        //            Console.Write("User not found, let's create him/her. Enter password: ");
        //            var password = ReadPassword();
        //            Console.Write("Enter display name (optional): ");
        //            var displayName = Console.ReadLine();
        //            return await client.CreateUserWithEmailAndPasswordAsync(email, password, displayName);
        //        }
        //    }
        //    catch (FirebaseAuthException ex)
        //    {
        //        Console.WriteLine($"Exception thrown: {ex.Reason}");
        //        throw;
        //    }
        //}

        //private static string ReadPassword()
        //{
        //    var pass = "";
        //    do
        //    {
        //        ConsoleKeyInfo key = Console.ReadKey(true);
        //        // Backspace Should Not Work
        //        if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
        //        {
        //            pass += key.KeyChar;
        //            Console.Write("*");
        //        }
        //        else
        //        {
        //            if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
        //            {
        //                pass = pass.Substring(0, (pass.Length - 1));
        //                Console.Write("\b \b");
        //            }
        //            else if (key.Key == ConsoleKey.Enter)
        //            {
        //                break;
        //            }
        //        }
        //    } while (true);

        //    Console.WriteLine();

        //    return pass;
        //}
    }
}

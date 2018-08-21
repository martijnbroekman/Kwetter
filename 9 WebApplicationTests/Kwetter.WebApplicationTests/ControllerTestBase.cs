using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kwetter.WebApplicationTests
{
    public abstract class ControllerTestBase : IDisposable
    {
        private readonly TestServer server;

        protected HttpClient Client { get; }

        protected enum Role
        {
            User,
            Moderator,
            Administrator,
            Anonymous
        }

        public ControllerTestBase()
        {
            var host = new WebHostBuilder()
                            .UseStartup<TestStartup>()
                            .UseEnvironment("Test");

            server = new TestServer(host);
            Client = server.CreateClient();
        }

        protected async Task SetTokenForRole(Role role)
        {
            if (!role.Equals(Role.Anonymous))
            {
                var dict = new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "username", Enum.GetName(typeof(Role), role) },
                { "password", "Welcome1!" },
                { "scope", "profile openid email offline_access roles" }
            };

                var req = new HttpRequestMessage(HttpMethod.Post, "/connect/token") { Content = new FormUrlEncodedContent(dict) };
                var res = await Client.SendAsync(req);
                var jsonResponse = JObject.Parse(await res.Content.ReadAsStringAsync());
                var token = jsonResponse["access_token"].Value<string>();

                Client.DefaultRequestHeaders.Add("Access_Token", $"bearer {token}");
            }
            
        }

        protected async Task<T> GetAsync<T>(string url, Role role = Role.User)
        {
            await SetTokenForRole(role);

            return JsonConvert.DeserializeObject<T>(await Client.GetStringAsync(url));
        }
        
        public void Dispose()
        {
            Client.Dispose();
            server.Dispose();
        }
    }
}

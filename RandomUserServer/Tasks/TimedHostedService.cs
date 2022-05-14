using Newtonsoft.Json;
using RandomUserServer.Models;
using System.Net.Http.Headers;

namespace RandomUserServer.Tasks
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private Timer _timer = null!;
        private readonly IServiceScopeFactory _scopeFactory;

        public TimedHostedService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        async Task<RandomUserModel> RequestToApi()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://randomuser.me");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "/api");
            var responseTask = await client.SendAsync(request);

            string response = await responseTask.Content.ReadAsStringAsync();
            response = response.Substring(response.IndexOf("[") + 1);
            response = response.Substring(0, response.IndexOf("]"));

            return JsonConvert.DeserializeObject<RandomUserModel>(response)!;
        }
        private async void DoWork(object? state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                try
                {
                    Context _context = scope.ServiceProvider.GetRequiredService<Context>();
                    RandomUserModel model = await RequestToApi();
                    string CountryName = model.Location.country;
                    string UserName = model.Login.username;

                    var country = await _context.Countries.FindAsync(CountryName);
                    var user = await _context.Users.FindAsync(UserName);
                    if (user != null)
                    {
                        user = _context.Users.Find(UserName)!;
                    }
                    else
                    {
                        user = new User()
                        {
                            Email = model.Email,
                            Gender = model.Gender,
                            Id = UserName
                        };
                        _context.Users.Add(user);
                        _context.SaveChanges();
                        user = _context.Users.Find(user.Id)!;
                    }
                    if (country != null)
                    {
                        country.Users.Add(user);
                    }
                    else
                    {
                        country = new Country()
                        {
                            Id = model.Location.country
                        };
                        user = _context.Users.Find(user.Id)!;
                        country.Users.Add(user);
                        _context.Countries.Add(country);
                    }
                    _context.SaveChanges();

                }
                catch
                {

                }
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}

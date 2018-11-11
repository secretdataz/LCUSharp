using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LCUSharp
{
    public class LeagueClient : ILeagueClient
    {
        private string ApiUri;
        private string LeaguePath;
        private int LeaguePid;

        public event LeagueClosedHandler LeagueClosed;

        private HttpClient httpClient;

        private RuneManager RuneManager = null;

        private Summoners Summoners = null;

        public string Token { get; set; }
        public ushort Port { get; set; }

        private LeagueClient()
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            httpClient = new HttpClient(handler);
        }

        public static async Task<ILeagueClient> Connect()
        {
            Process process = await FindLeagueAsync();
            var ret = (LeagueClient) await Connect(Path.GetDirectoryName(process.MainModule.FileName));
            ret.LeaguePid = process.Id;

            return ret;
        }

        public static async Task<ILeagueClient> Connect(string path)
        {
            var ret = new LeagueClient();
            ret.LeaguePath = path;

            await ret.WatchLockFileAsync();

            var process = Process.GetProcessById(ret.LeaguePid);
            process.EnableRaisingEvents = true;
            process.Exited += ret.League_Exited;

            return ret;
        }

        public async Task WatchLockFileAsync()
        {
            var lockFilePath = Path.Combine(LeaguePath, "lockfile");

            if (File.Exists(lockFilePath))
            {
                await ParseLockFileAsync(lockFilePath);
                return;
            }

            var tcs = new TaskCompletionSource<bool>();
            var watcher = new FileSystemWatcher(LeaguePath);
            FileSystemEventHandler handler = null;
            handler = async (s, e) =>
            {
                if(e.Name.Equals("lockfile"))
                {
                    await ParseLockFileAsync(lockFilePath);
                    tcs.TrySetResult(true);
                    watcher.Created -= handler;
                    watcher.Dispose();
                }
            };

            watcher.Created += handler;
            watcher.EnableRaisingEvents = true;

            return;
        }

        private async Task ParseLockFileAsync(string lockFile)
        {
            using (var fileStream = new FileStream(lockFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(fileStream))
            {
                var text = await reader.ReadToEndAsync();
                string[] items = text.Split(':');
                Token = items[3];
                Port = ushort.Parse(items[2]);
                ApiUri = "https://127.0.0.1:" + Port.ToString() + "/";

                var bytes = Encoding.ASCII.GetBytes("riot:" + Token);

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"riot:{Token}")));
                httpClient.BaseAddress = new Uri(ApiUri);

                if(this.LeaguePid == 0)
                {
                    this.LeaguePid = int.Parse(items[1]);
                }
            }
        }

        private static async Task<Process> FindLeagueAsync()
        {
            Process ret = null;

            await Task.Run(() =>
            {
                while (true)
                {
                    var processes = Process.GetProcessesByName("LeagueClientUx");
                    if (processes.Length > 0)
                    {
                        ret = processes[0];
                        break;
                    }
                    Thread.Sleep(1000);
                }
            });

            return ret;
        }

        private void League_Exited(object sender, EventArgs e)
        {
            LeagueClosed();
        }

        public RuneManager GetRuneManager()
        {
            if (RuneManager == null)
                RuneManager = new RuneManager(this);
            return RuneManager;
        }

        public HttpClient GetHttpClient()
        {
            return httpClient;
        }

        public async Task<HttpResponseMessage> MakeApiRequest(HttpMethod method, string endpoint, object data = null)
        {
            var json = data == null ? "" : JsonConvert.SerializeObject(data);
            switch (method)
            {
                case HttpMethod.Get:
                    return await httpClient.GetAsync(endpoint);
                case HttpMethod.Post:
                    return await httpClient.PostAsync(endpoint, new StringContent(json, Encoding.UTF8, "application/json"));
                case HttpMethod.Put:
                    return await httpClient.PutAsync(endpoint, new StringContent(json, Encoding.UTF8, "application/json"));
                case HttpMethod.Delete:
                    return await httpClient.DeleteAsync(endpoint);
                default:
                    throw new Exception("Unsupported HTTP method");
            }
        }

        public async Task<T> MakeApiRequestAs<T>(HttpMethod method, string endpoint, object data = null)
        {
            var response = await MakeApiRequest(method, endpoint, data);
            T responseObject = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            return responseObject;
        }

        public Summoners GetSummonersModule()
        {
            if (Summoners == null)
                Summoners = new Summoners(this);
            return Summoners;
        }
    }
}

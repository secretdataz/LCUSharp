using EasyHttp.Http;
using LCUSharp.DataObjects;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LCUSharp
{
    public class LeagueClient : ILeagueClient
    {
        private string _Token;
        private ushort _Port;
        private string ApiUri;
        private string LeaguePath;
        private int LeaguePid;

        public event LeagueClosedHandler LeagueClosed;

        private HttpClient httpClient = new HttpClient();

        public string Token { get => _Token; set => _Token = value; }
        public ushort Port { get => _Port; set => _Port = value; }

        private LeagueClient()
        {

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
                ApiUri = "https://localhost:" + Port.ToString() + "/";

                var bytes = Encoding.ASCII.GetBytes("riot:" + Token);

                httpClient.Request.Accept = HttpContentTypes.ApplicationJson;
                httpClient.Request.ContentType = HttpContentTypes.ApplicationJson;
                httpClient.Request.SetBasicAuthentication("riot", Token);

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

        public RunePage GetCurrentRunePage()
        {
            var response = httpClient.Get(ApiUri + "lol-perks/v1/currentpage");
            var page = response.StaticBody<RunePage>();
            return page;
        }
        
        public RunePage[] GetRunePages()
        {
            var response = httpClient.Get(ApiUri + "lol-perks/v1/pages");
            var pages = response.StaticBody<RunePage[]>();
            return pages;
        }

        public AddRuneResult AddRunePage(RunePage page)
        {
            var response = httpClient.Post(ApiUri + "lol-perks/v1/pages", page, HttpContentTypes.ApplicationJson);
            if(response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var error = response.StaticBody<Error>();
                if (error.message.Equals("Max pages reached"))
                    return AddRuneResult.MaxPageReached;
            }
            return AddRuneResult.Success;
        }
    }
}

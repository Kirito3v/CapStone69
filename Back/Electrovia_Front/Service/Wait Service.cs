namespace Electrovia_Front.Service
{
    public static class Wait_Service
    {
        public static async Task WaitForService(string url, TimeSpan retryDelay)
        {
            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(2) };
            while (true)
            {
                try
                {
                    var res = await client.GetAsync(url);
                    if (res.IsSuccessStatusCode) break;
                }
                catch
                {
                    // ignored
                }

                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Waiting for {url}…");
                await Task.Delay(retryDelay);
            }
        }
    }
}

using System.Net.Http.Headers;

using (var client = new HttpClient())
{
    client.BaseAddress = new Uri("http://localhost:5000/");
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    var runnerFirst = RunRequests(1);
    var runnerSecond = RunRequests(2);
    var runnerThird = RunRequests(3);

    await Task.WhenAll(Task.Run(() => runnerFirst), Task.Run(() => runnerSecond), Task.Run(() => runnerThird));

    async Task RunRequest(int id, string resourceId)
    {
        HttpResponseMessage response = await client.GetAsync(resourceId);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"From {id} {resourceId} is {result} on thread {Thread.CurrentThread.ManagedThreadId}");
        }
    }

    async Task RunRequests(int id)
    {
        try
        {
            int i = 0;
            int count = int.MaxValue;

            while (i < count)
            {
                Console.WriteLine($"Starting running method with id {id} on thread {Thread.CurrentThread.ManagedThreadId}");
                await RunRequest(id, "naturalNumberCurrent");
                await RunRequest(id, "naturalNumberNext");
                await RunRequest(id, "templateCurrent");
                await RunRequest(id, "templateNext");
                Console.WriteLine($"Finishing running method with id {id} on thread {Thread.CurrentThread.ManagedThreadId}");
                i++;
            }
        }
        catch(Exception ex) { Console.WriteLine(ex.ToString()); }
    }
}

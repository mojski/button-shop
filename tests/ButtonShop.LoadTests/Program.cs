using ButtonShop.LoadTests;

var tokenSource = new CancellationTokenSource();
var cancellationToken = tokenSource.Token;

// ctrl + c
Console.CancelKeyPress += (_, ea) =>
{
    ea.Cancel = true;
    tokenSource.Cancel();
};

tokenSource.CancelAfter(TimeSpan.FromSeconds(50));

var baseUrl = args.FirstOrDefault(arg => arg.StartsWith("--baseUrl="))?
    .Split("=", 2)[1] ?? "http://localhost:5080";


try
{
    await OrderTests.Run(baseUrl, cancellationToken);
}
catch (TaskCanceledException tce)
{
    Console.WriteLine(tce.Message, "Load tests was canceled");
}
catch (Exception e)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Fatal Error: {e.Message}");
    Console.ForegroundColor = ConsoleColor.White;
}

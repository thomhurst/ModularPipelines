namespace ModularPipelines.Engine;

internal class EngineCancellationToken : CancellationTokenSource
{
    public EngineCancellationToken()
    {
        Console.CancelKeyPress += (_, _) =>
        {
            Cancel();
        };

        AppDomain.CurrentDomain.ProcessExit += (_, _) =>
        {
            Cancel();
        };
    }
}

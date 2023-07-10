namespace ModularPipelines.Engine;

internal class EngineCancellationToken : CancellationTokenSource
{
    private bool _disposed;

    public EngineCancellationToken()
    {
        Console.CancelKeyPress += (_, _) =>
        {
            if(!_disposed)
            {
                Cancel();
            }
        };

        AppDomain.CurrentDomain.ProcessExit += (_, _) =>
        {
            if(!_disposed)
            {
                Cancel();
            }
        };
    }

    protected override void Dispose(bool disposing)
    {
        _disposed = true;
        base.Dispose(disposing);
    }
}

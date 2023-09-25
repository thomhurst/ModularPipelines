using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Engine;

[ExcludeFromCodeCoverage]
internal class EngineCancellationToken : CancellationTokenSource
{
    private bool _disposed;

    public EngineCancellationToken()
    {
        Console.CancelKeyPress += (_, _) => TryCancel();

        AppDomain.CurrentDomain.ProcessExit += (_, _) => TryCancel();
    }

    protected override void Dispose(bool disposing)
    {
        _disposed = true;
        base.Dispose(disposing);
    }

    private void TryCancel()
    {
        if (!_disposed && Token.CanBeCanceled)
        {
            Cancel();
        }
    }
}
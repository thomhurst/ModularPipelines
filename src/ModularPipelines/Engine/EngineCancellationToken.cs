using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Engine;

[ExcludeFromCodeCoverage]
internal class EngineCancellationToken : CancellationTokenSource
{
    public string? Reason { get; set; }

    private bool _disposed;

    public EngineCancellationToken()
    {
        Console.CancelKeyPress += (_, _) => TryCancel();

        AppDomain.CurrentDomain.ProcessExit += (_, _) => TryCancel();
    }

    public void CancelWithReason(string? reason)
    {
        Reason = reason;
        Cancel();
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
using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Engine;

[ExcludeFromCodeCoverage]
internal class EngineCancellationToken : CancellationTokenSource
{
    public string? Reason { get; set; }

    private bool _isCancelled;

    public bool IsCancelled =>
        _isCancelled || IsCancellationRequested || Token.IsCancellationRequested || Reason != null;

    private bool _disposed;

    [ExcludeFromCodeCoverage]
    public EngineCancellationToken()
    {
        Console.CancelKeyPress += (_, args) =>
        {
            args.Cancel = true;
            TryCancel();
        };
        
        AppDomain.CurrentDomain.ProcessExit += (_, _) => TryCancel();
    }

    public void CancelWithReason(string? reason)
    {
        Reason = reason;
        _isCancelled = true;
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
            _isCancelled = true;
            Cancel();
        }
    }
}
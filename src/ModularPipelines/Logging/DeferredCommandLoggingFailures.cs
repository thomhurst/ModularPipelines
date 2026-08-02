using System.Runtime.ExceptionServices;

namespace ModularPipelines.Logging;

internal sealed class DeferredCommandLoggingFailures
{
    private readonly List<Exception> _failures = [];

    public bool HasFailures => _failures.Count > 0;

    public void Capture(Action log)
    {
        try
        {
            log();
        }
        catch (Exception exception)
        {
            _failures.Add(exception);
        }
    }

    public Exception CombineWith(Exception primaryFailure) =>
        _failures.Count == 0
            ? primaryFailure
            : new AggregateException([primaryFailure, .. _failures]);

    public void Throw()
    {
        if (_failures.Count == 1)
        {
            ExceptionDispatchInfo.Capture(_failures[0]).Throw();
        }

        if (_failures.Count > 1)
        {
            throw new AggregateException(_failures);
        }
    }
}

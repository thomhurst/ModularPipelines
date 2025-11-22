using System.Runtime.ExceptionServices;

namespace ModularPipelines.Engine;

/// <summary>
/// Collects secondary exceptions that occur during pipeline execution,
/// such as failures in AlwaysRun modules or dependency resolution.
/// The primary/first exception that fails the pipeline is stored in EngineCancellationToken.
/// </summary>
internal class SecondaryExceptionContainer : ISecondaryExceptionContainer
{
    private readonly List<ExceptionDispatchInfo> _exceptions = [];

    public void RegisterException(Exception exception)
    {
        _exceptions.Add(ExceptionDispatchInfo.Capture(exception));
    }

    public void ThrowExceptions()
    {
        if (_exceptions.Count == 1)
        {
            _exceptions.First().Throw();
        }

        if (_exceptions.Any())
        {
            throw new AggregateException(_exceptions.Select(e => e.SourceException));
        }
    }
}

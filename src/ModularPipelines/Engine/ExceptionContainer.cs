namespace ModularPipelines.Engine;

internal class ExceptionContainer : IExceptionContainer
{
    private readonly List<Exception> _exceptions = [];
    
    public void RegisterException(Exception exception)
    {
        _exceptions.Add(exception);
    }

    public void ThrowExceptions()
    {
        if (_exceptions.Count == 1)
        {
            throw _exceptions.First();
        }

        if (_exceptions.Any())
        {
            throw new AggregateException(_exceptions);
        }
    }
}
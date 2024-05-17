namespace ModularPipelines.Engine;

internal interface IExceptionContainer
{
    void RegisterException(Exception exception);
    
    void ThrowExceptions();
}
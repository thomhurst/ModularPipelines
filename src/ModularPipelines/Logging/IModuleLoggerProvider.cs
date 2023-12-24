namespace ModularPipelines.Logging;

public interface IModuleLoggerProvider
{
    IModuleLogger GetLogger();
    
    internal IModuleLogger GetLogger(Type type);
}
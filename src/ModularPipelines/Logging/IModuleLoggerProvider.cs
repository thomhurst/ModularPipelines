namespace ModularPipelines.Logging;

public interface IModuleLoggerProvider
{
    internal IModuleLogger GetLogger(Type type);

    IModuleLogger GetLogger();
}

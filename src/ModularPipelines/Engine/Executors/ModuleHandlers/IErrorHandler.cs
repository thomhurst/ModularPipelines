namespace ModularPipelines.Engine.Executors.ModuleHandlers;

internal interface IErrorHandler
{
    Task Handle(Exception exception);
}
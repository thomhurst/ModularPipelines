namespace ModularPipelines.Engine.Executors;

internal interface IPrintProgressExecutor : IAsyncDisposable
{
    Task InitializeAsync();
}
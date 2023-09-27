namespace ModularPipelines.Helpers;

internal class PipelineDisposer : IPipelineDisposer
{
    private readonly IServiceProvider _serviceProvider;

    public PipelineDisposer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task DisposeAsync()
    {
        await Disposer.DisposeObjectAsync(_serviceProvider);
    }
}
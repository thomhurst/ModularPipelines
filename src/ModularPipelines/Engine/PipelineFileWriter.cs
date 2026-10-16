using EnumerableAsyncProcessor.Extensions;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Interfaces;

namespace ModularPipelines.Engine;

internal class PipelineFileWriter : IPipelineFileWriter, IScopeDisposer
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IPipelineContextProvider _pipelineContextProvider;
    private readonly IEnumerable<IBuildSystemPipelineFileWriter> _writers;
    private readonly List<IServiceScope> _scopes = [];

    public PipelineFileWriter(IServiceProvider serviceProvider, IPipelineContextProvider pipelineContextProvider)
    {
        _serviceProvider = serviceProvider;
        _pipelineContextProvider = pipelineContextProvider;
        var scope = serviceProvider.CreateScope();
        _scopes.Add(scope);
        _writers = scope.ServiceProvider.GetServices<IBuildSystemPipelineFileWriter>();
    }

    public async Task WritePipelineFiles()
    {
        await _writers
            .ForEachAsync(x =>
                x.WriteAsync(_pipelineContextProvider.GetModuleContext(x))
            )
            .ProcessInParallel()
            .WaitAsync()
            .ConfigureAwait(false);
    }

    public IEnumerable<IServiceScope> GetScopes()
    {
        return _scopes;
    }
}

using EnumerableAsyncProcessor.Extensions;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Interfaces;

namespace ModularPipelines.Engine;

internal class PipelineFileWriter : IPipelineFileWriter, IScopeDisposer
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IPipelineContextProvider _pipelineContextProvider;
    private readonly IEnumerable<IBuildSystemPipelineFileWriter> _writers;
    private readonly List<IServiceScope> _scopes = new();

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
                x.Write(_pipelineContextProvider.GetModuleContext())
            )
            .ProcessInParallel();
    }

    public IEnumerable<IServiceScope> GetScopes()
    {
        return _scopes;
    }
}
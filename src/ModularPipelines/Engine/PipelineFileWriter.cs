using EnumerableAsyncProcessor.Extensions;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Interfaces;

namespace ModularPipelines.Engine;

internal class PipelineFileWriter : IPipelineFileWriter
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IEnumerable<IBuildSystemPipelineFileWriter> _writers;

    public PipelineFileWriter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        var scope = serviceProvider.CreateScope();
        _writers = scope.ServiceProvider.GetServices<IBuildSystemPipelineFileWriter>();
    }

    public async Task WritePipelineFiles()
    {
        await _writers
            .ForEachAsync(x =>
                x.Write(_serviceProvider.GetRequiredService<IPipelineContext>())
            )
            .ProcessInParallel();
    }
}
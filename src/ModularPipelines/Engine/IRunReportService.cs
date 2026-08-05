using ModularPipelines.Models;

namespace ModularPipelines.Engine;

internal interface IRunReportService
{
    Task<PipelineRunReport> CompleteAsync(
        PipelineSummary summary,
        Exception? pipelineException = null,
        CancellationToken cancellationToken = default);
}

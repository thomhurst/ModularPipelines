using ModularPipelines.Models;
using ModularPipelines.Reporting;

namespace ModularPipelines.Engine;

internal interface IRunReportService
{
    Task<PipelineRunReport> CompleteAsync(
        PipelineSummary summary,
        Exception? pipelineException = null,
        CancellationToken cancellationToken = default);
}

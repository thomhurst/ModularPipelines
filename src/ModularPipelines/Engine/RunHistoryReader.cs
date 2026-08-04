using Microsoft.Extensions.Options;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

internal sealed class RunHistoryReader(
    IRunHistoryStore historyStore,
    IOptions<PipelineOptions> pipelineOptions) : IRunHistoryReader
{
    public async Task<IReadOnlyList<ModuleDurationSample>> GetModuleDurationTrendAsync(
        string moduleTypeName,
        int lastN,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(moduleTypeName);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(lastN);

        var pipelineIdentity = pipelineOptions.Value.RunReport.PipelineIdentity;
        if (string.IsNullOrWhiteSpace(pipelineIdentity))
        {
            throw new InvalidOperationException(
                $"{nameof(RunReportOptions.PipelineIdentity)} must be configured to query run history.");
        }

        var samples = new List<ModuleDurationSample>();
        await foreach (var report in historyStore.GetRunsAsync(
                               new RunHistoryQuery
                               {
                                   PipelineIdentity = pipelineIdentity,
                                   MaxRuns = lastN,
                               },
                               cancellationToken)
                           .ConfigureAwait(false))
        {
            var module = report.Modules.FirstOrDefault(candidate =>
                string.Equals(candidate.ModuleTypeName, moduleTypeName, StringComparison.Ordinal));
            if (module is not { DurationMeasured: true } || string.IsNullOrWhiteSpace(report.RunId))
            {
                continue;
            }

            samples.Add(new ModuleDurationSample(
                report.RunId,
                report.End,
                module.Status,
                module.Duration));
        }

        return samples;
    }
}

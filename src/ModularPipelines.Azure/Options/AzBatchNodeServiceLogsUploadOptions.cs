using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("batch", "node", "service-logs", "upload")]
public record AzBatchNodeServiceLogsUploadOptions(
[property: CliOption("--node-id")] string NodeId,
[property: CliOption("--pool-id")] string PoolId
) : AzOptions
{
    [CliOption("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--container-url")]
    public string? ContainerUrl { get; set; }

    [CliOption("--end-time")]
    public string? EndTime { get; set; }

    [CliOption("--json-file")]
    public string? JsonFile { get; set; }

    [CliOption("--resource-id")]
    public string? ResourceId { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage-mover", "job-definition", "start-job")]
public record AzStorageMoverJobDefinitionStartJobOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--job-definition-name")]
    public string? JobDefinitionName { get; set; }

    [CliOption("--project-name")]
    public string? ProjectName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--storage-mover-name")]
    public string? StorageMoverName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}
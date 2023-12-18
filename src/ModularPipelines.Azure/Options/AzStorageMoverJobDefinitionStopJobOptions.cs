using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage-mover", "job-definition", "stop-job")]
public record AzStorageMoverJobDefinitionStopJobOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--job-definition-name")]
    public string? JobDefinitionName { get; set; }

    [CommandSwitch("--project-name")]
    public string? ProjectName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--storage-mover-name")]
    public string? StorageMoverName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}
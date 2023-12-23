using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr-containers", "list-job-runs")]
public record AwsEmrContainersListJobRunsOptions(
[property: CommandSwitch("--virtual-cluster-id")] string VirtualClusterId
) : AwsOptions
{
    [CommandSwitch("--created-before")]
    public long? CreatedBefore { get; set; }

    [CommandSwitch("--created-after")]
    public long? CreatedAfter { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--states")]
    public string[]? States { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("outposts", "list-outposts")]
public record AwsOutpostsListOutpostsOptions : AwsOptions
{
    [CommandSwitch("--life-cycle-status-filter")]
    public string[]? LifeCycleStatusFilter { get; set; }

    [CommandSwitch("--availability-zone-filter")]
    public string[]? AvailabilityZoneFilter { get; set; }

    [CommandSwitch("--availability-zone-id-filter")]
    public string[]? AvailabilityZoneIdFilter { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
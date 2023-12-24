using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "list-jobs")]
public record AwsIotListJobsOptions : AwsOptions
{
    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--target-selection")]
    public string? TargetSelection { get; set; }

    [CommandSwitch("--thing-group-name")]
    public string? ThingGroupName { get; set; }

    [CommandSwitch("--thing-group-id")]
    public string? ThingGroupId { get; set; }

    [CommandSwitch("--namespace-id")]
    public string? NamespaceId { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
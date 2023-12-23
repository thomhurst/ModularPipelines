using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resiliencehub", "update-resiliency-policy")]
public record AwsResiliencehubUpdateResiliencyPolicyOptions(
[property: CommandSwitch("--policy-arn")] string PolicyArn
) : AwsOptions
{
    [CommandSwitch("--data-location-constraint")]
    public string? DataLocationConstraint { get; set; }

    [CommandSwitch("--policy")]
    public IEnumerable<KeyValue>? Policy { get; set; }

    [CommandSwitch("--policy-description")]
    public string? PolicyDescription { get; set; }

    [CommandSwitch("--policy-name")]
    public string? PolicyName { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
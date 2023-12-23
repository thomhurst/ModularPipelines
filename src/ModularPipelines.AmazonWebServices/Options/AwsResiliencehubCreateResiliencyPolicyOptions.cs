using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resiliencehub", "create-resiliency-policy")]
public record AwsResiliencehubCreateResiliencyPolicyOptions(
[property: CommandSwitch("--policy")] IEnumerable<KeyValue> Policy,
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--tier")] string Tier
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--data-location-constraint")]
    public string? DataLocationConstraint { get; set; }

    [CommandSwitch("--policy-description")]
    public string? PolicyDescription { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
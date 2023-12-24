using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "create-traffic-policy-version")]
public record AwsRoute53CreateTrafficPolicyVersionOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--document")] string Document
) : AwsOptions
{
    [CommandSwitch("--comment")]
    public string? Comment { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
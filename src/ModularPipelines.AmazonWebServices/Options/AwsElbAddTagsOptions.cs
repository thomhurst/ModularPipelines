using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elb", "add-tags")]
public record AwsElbAddTagsOptions(
[property: CommandSwitch("--load-balancer-names")] string[] LoadBalancerNames,
[property: CommandSwitch("--tags")] string[] Tags
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
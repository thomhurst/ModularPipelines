using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wafv2", "list-resources-for-web-acl")]
public record AwsWafv2ListResourcesForWebAclOptions(
[property: CommandSwitch("--web-acl-arn")] string WebAclArn
) : AwsOptions
{
    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
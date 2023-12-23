using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("waf", "create-web-acl")]
public record AwsWafCreateWebAclOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--metric-name")] string MetricName,
[property: CommandSwitch("--default-action")] string DefaultAction,
[property: CommandSwitch("--change-token")] string ChangeToken
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
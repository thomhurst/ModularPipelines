using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("waf-regional", "update-web-acl")]
public record AwsWafRegionalUpdateWebAclOptions(
[property: CommandSwitch("--web-acl-id")] string WebAclId,
[property: CommandSwitch("--change-token")] string ChangeToken
) : AwsOptions
{
    [CommandSwitch("--updates")]
    public string[]? Updates { get; set; }

    [CommandSwitch("--default-action")]
    public string? DefaultAction { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
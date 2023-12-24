using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "update-entitlement")]
public record AwsAppstreamUpdateEntitlementOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--stack-name")] string StackName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--app-visibility")]
    public string? AppVisibility { get; set; }

    [CommandSwitch("--attributes")]
    public string[]? Attributes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "create-entitlement")]
public record AwsAppstreamCreateEntitlementOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--stack-name")] string StackName,
[property: CommandSwitch("--app-visibility")] string AppVisibility,
[property: CommandSwitch("--attributes")] string[] Attributes
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
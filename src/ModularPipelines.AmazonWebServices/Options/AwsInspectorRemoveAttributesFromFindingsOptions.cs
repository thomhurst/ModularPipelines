using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("inspector", "remove-attributes-from-findings")]
public record AwsInspectorRemoveAttributesFromFindingsOptions(
[property: CommandSwitch("--finding-arns")] string[] FindingArns,
[property: CommandSwitch("--attribute-keys")] string[] AttributeKeys
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
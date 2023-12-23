using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("inspector", "add-attributes-to-findings")]
public record AwsInspectorAddAttributesToFindingsOptions(
[property: CommandSwitch("--finding-arns")] string[] FindingArns,
[property: CommandSwitch("--attributes")] string[] Attributes
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
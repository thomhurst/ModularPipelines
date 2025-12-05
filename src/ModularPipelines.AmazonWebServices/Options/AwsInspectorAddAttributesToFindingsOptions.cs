using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector", "add-attributes-to-findings")]
public record AwsInspectorAddAttributesToFindingsOptions(
[property: CliOption("--finding-arns")] string[] FindingArns,
[property: CliOption("--attributes")] string[] Attributes
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
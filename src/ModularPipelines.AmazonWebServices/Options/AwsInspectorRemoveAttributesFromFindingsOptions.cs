using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector", "remove-attributes-from-findings")]
public record AwsInspectorRemoveAttributesFromFindingsOptions(
[property: CliOption("--finding-arns")] string[] FindingArns,
[property: CliOption("--attribute-keys")] string[] AttributeKeys
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
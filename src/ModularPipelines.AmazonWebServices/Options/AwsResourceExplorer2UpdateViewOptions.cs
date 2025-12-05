using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-explorer-2", "update-view")]
public record AwsResourceExplorer2UpdateViewOptions(
[property: CliOption("--view-arn")] string ViewArn
) : AwsOptions
{
    [CliOption("--filters")]
    public string? Filters { get; set; }

    [CliOption("--included-properties")]
    public string[]? IncludedProperties { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
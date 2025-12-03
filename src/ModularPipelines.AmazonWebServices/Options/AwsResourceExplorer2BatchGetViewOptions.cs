using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-explorer-2", "batch-get-view")]
public record AwsResourceExplorer2BatchGetViewOptions : AwsOptions
{
    [CliOption("--view-arns")]
    public string[]? ViewArns { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
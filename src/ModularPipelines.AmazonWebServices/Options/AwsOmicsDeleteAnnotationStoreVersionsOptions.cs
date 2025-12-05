using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("omics", "delete-annotation-store-versions")]
public record AwsOmicsDeleteAnnotationStoreVersionsOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--versions")] string[] Versions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
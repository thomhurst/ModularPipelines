using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("omics", "wait", "annotation-store-version-deleted")]
public record AwsOmicsWaitAnnotationStoreVersionDeletedOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--version-name")] string VersionName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
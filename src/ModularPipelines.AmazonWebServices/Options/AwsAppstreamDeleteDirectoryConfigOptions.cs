using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "delete-directory-config")]
public record AwsAppstreamDeleteDirectoryConfigOptions(
[property: CliOption("--directory-name")] string DirectoryName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
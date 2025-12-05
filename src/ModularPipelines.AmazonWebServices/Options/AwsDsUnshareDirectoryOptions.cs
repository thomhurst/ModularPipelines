using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "unshare-directory")]
public record AwsDsUnshareDirectoryOptions(
[property: CliOption("--directory-id")] string DirectoryId,
[property: CliOption("--unshare-target")] string UnshareTarget
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
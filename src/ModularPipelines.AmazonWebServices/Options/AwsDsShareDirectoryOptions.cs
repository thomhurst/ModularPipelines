using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "share-directory")]
public record AwsDsShareDirectoryOptions(
[property: CliOption("--directory-id")] string DirectoryId,
[property: CliOption("--share-target")] string ShareTarget,
[property: CliOption("--share-method")] string ShareMethod
) : AwsOptions
{
    [CliOption("--share-notes")]
    public string? ShareNotes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
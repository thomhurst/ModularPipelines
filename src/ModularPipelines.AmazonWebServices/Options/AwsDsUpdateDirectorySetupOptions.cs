using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "update-directory-setup")]
public record AwsDsUpdateDirectorySetupOptions(
[property: CliOption("--directory-id")] string DirectoryId,
[property: CliOption("--update-type")] string UpdateType
) : AwsOptions
{
    [CliOption("--os-update-settings")]
    public string? OsUpdateSettings { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
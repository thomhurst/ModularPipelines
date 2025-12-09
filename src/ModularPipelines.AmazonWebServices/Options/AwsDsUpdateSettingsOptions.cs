using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "update-settings")]
public record AwsDsUpdateSettingsOptions(
[property: CliOption("--directory-id")] string DirectoryId,
[property: CliOption("--settings")] string[] Settings
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
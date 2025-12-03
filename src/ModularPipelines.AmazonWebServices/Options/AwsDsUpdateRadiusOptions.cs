using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "update-radius")]
public record AwsDsUpdateRadiusOptions(
[property: CliOption("--directory-id")] string DirectoryId,
[property: CliOption("--radius-settings")] string RadiusSettings
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
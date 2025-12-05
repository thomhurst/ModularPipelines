using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appconfig", "delete-extension")]
public record AwsAppconfigDeleteExtensionOptions(
[property: CliOption("--extension-identifier")] string ExtensionIdentifier
) : AwsOptions
{
    [CliOption("--version-number")]
    public int? VersionNumber { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
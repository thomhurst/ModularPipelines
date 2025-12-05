using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appconfig", "create-extension-association")]
public record AwsAppconfigCreateExtensionAssociationOptions(
[property: CliOption("--extension-identifier")] string ExtensionIdentifier,
[property: CliOption("--resource-identifier")] string ResourceIdentifier
) : AwsOptions
{
    [CliOption("--extension-version-number")]
    public int? ExtensionVersionNumber { get; set; }

    [CliOption("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
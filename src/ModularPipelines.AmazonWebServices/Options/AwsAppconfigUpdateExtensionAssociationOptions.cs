using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appconfig", "update-extension-association")]
public record AwsAppconfigUpdateExtensionAssociationOptions(
[property: CliOption("--extension-association-id")] string ExtensionAssociationId
) : AwsOptions
{
    [CliOption("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
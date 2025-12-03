using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workdocs", "create-custom-metadata")]
public record AwsWorkdocsCreateCustomMetadataOptions(
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--custom-metadata")] IEnumerable<KeyValue> CustomMetadata
) : AwsOptions
{
    [CliOption("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CliOption("--version-id")]
    public string? VersionId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workdocs", "delete-custom-metadata")]
public record AwsWorkdocsDeleteCustomMetadataOptions(
[property: CliOption("--resource-id")] string ResourceId
) : AwsOptions
{
    [CliOption("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CliOption("--version-id")]
    public string? VersionId { get; set; }

    [CliOption("--keys")]
    public string[]? Keys { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
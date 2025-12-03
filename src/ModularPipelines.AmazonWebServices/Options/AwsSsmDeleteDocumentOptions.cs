using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "delete-document")]
public record AwsSsmDeleteDocumentOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--document-version")]
    public string? DocumentVersion { get; set; }

    [CliOption("--version-name")]
    public string? VersionName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
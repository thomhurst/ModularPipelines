using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "get-document")]
public record AwsSsmGetDocumentOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--version-name")]
    public string? VersionName { get; set; }

    [CliOption("--document-version")]
    public string? DocumentVersion { get; set; }

    [CliOption("--document-format")]
    public string? DocumentFormat { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
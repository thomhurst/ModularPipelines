using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "update-document")]
public record AwsSsmUpdateDocumentOptions(
[property: CliOption("--content")] string Content,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--attachments")]
    public string[]? Attachments { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--version-name")]
    public string? VersionName { get; set; }

    [CliOption("--document-version")]
    public string? DocumentVersion { get; set; }

    [CliOption("--document-format")]
    public string? DocumentFormat { get; set; }

    [CliOption("--target-type")]
    public string? TargetType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
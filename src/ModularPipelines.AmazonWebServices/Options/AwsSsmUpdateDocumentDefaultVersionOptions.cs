using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "update-document-default-version")]
public record AwsSsmUpdateDocumentDefaultVersionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--document-version")] string DocumentVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
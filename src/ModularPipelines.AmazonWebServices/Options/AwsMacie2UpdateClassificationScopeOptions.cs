using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("macie2", "update-classification-scope")]
public record AwsMacie2UpdateClassificationScopeOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--s3")]
    public string? S3 { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
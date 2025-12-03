using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("macie2", "batch-get-custom-data-identifiers")]
public record AwsMacie2BatchGetCustomDataIdentifiersOptions : AwsOptions
{
    [CliOption("--ids")]
    public string[]? Ids { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
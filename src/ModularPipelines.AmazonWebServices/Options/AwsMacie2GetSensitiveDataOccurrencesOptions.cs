using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("macie2", "get-sensitive-data-occurrences")]
public record AwsMacie2GetSensitiveDataOccurrencesOptions(
[property: CliOption("--finding-id")] string FindingId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
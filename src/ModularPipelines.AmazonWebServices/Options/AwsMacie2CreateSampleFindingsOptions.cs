using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("macie2", "create-sample-findings")]
public record AwsMacie2CreateSampleFindingsOptions : AwsOptions
{
    [CliOption("--finding-types")]
    public string[]? FindingTypes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
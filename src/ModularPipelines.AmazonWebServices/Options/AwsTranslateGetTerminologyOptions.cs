using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("translate", "get-terminology")]
public record AwsTranslateGetTerminologyOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--terminology-data-format")]
    public string? TerminologyDataFormat { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
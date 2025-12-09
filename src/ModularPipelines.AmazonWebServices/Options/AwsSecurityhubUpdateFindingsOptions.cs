using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securityhub", "update-findings")]
public record AwsSecurityhubUpdateFindingsOptions(
[property: CliOption("--filters")] string Filters
) : AwsOptions
{
    [CliOption("--note")]
    public string? Note { get; set; }

    [CliOption("--record-state")]
    public string? RecordState { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
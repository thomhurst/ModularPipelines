using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeguru-security", "batch-get-findings")]
public record AwsCodeguruSecurityBatchGetFindingsOptions(
[property: CliOption("--finding-identifiers")] string[] FindingIdentifiers
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
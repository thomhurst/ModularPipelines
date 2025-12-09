using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securityhub", "batch-update-findings")]
public record AwsSecurityhubBatchUpdateFindingsOptions(
[property: CliOption("--finding-identifiers")] string[] FindingIdentifiers
) : AwsOptions
{
    [CliOption("--note")]
    public string? Note { get; set; }

    [CliOption("--severity")]
    public string? Severity { get; set; }

    [CliOption("--verification-state")]
    public string? VerificationState { get; set; }

    [CliOption("--confidence")]
    public int? Confidence { get; set; }

    [CliOption("--criticality")]
    public int? Criticality { get; set; }

    [CliOption("--types")]
    public string[]? Types { get; set; }

    [CliOption("--user-defined-fields")]
    public IEnumerable<KeyValue>? UserDefinedFields { get; set; }

    [CliOption("--workflow")]
    public string? Workflow { get; set; }

    [CliOption("--related-findings")]
    public string[]? RelatedFindings { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
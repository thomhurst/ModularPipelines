using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securityhub", "batch-update-findings")]
public record AwsSecurityhubBatchUpdateFindingsOptions(
[property: CommandSwitch("--finding-identifiers")] string[] FindingIdentifiers
) : AwsOptions
{
    [CommandSwitch("--note")]
    public string? Note { get; set; }

    [CommandSwitch("--severity")]
    public string? Severity { get; set; }

    [CommandSwitch("--verification-state")]
    public string? VerificationState { get; set; }

    [CommandSwitch("--confidence")]
    public int? Confidence { get; set; }

    [CommandSwitch("--criticality")]
    public int? Criticality { get; set; }

    [CommandSwitch("--types")]
    public string[]? Types { get; set; }

    [CommandSwitch("--user-defined-fields")]
    public IEnumerable<KeyValue>? UserDefinedFields { get; set; }

    [CommandSwitch("--workflow")]
    public string? Workflow { get; set; }

    [CommandSwitch("--related-findings")]
    public string[]? RelatedFindings { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
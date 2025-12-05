using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "get-ops-summary")]
public record AwsSsmGetOpsSummaryOptions : AwsOptions
{
    [CliOption("--sync-name")]
    public string? SyncName { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--aggregators")]
    public string[]? Aggregators { get; set; }

    [CliOption("--result-attributes")]
    public string[]? ResultAttributes { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
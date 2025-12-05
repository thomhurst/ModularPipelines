using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("trustedadvisor", "list-recommendations")]
public record AwsTrustedadvisorListRecommendationsOptions : AwsOptions
{
    [CliOption("--after-last-updated-at")]
    public long? AfterLastUpdatedAt { get; set; }

    [CliOption("--aws-service")]
    public string? AwsService { get; set; }

    [CliOption("--before-last-updated-at")]
    public long? BeforeLastUpdatedAt { get; set; }

    [CliOption("--check-identifier")]
    public string? CheckIdentifier { get; set; }

    [CliOption("--pillar")]
    public string? Pillar { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconvert", "create-job")]
public record AwsMediaconvertCreateJobOptions(
[property: CliOption("--role")] string Role,
[property: CliOption("--settings")] string Settings
) : AwsOptions
{
    [CliOption("--acceleration-settings")]
    public string? AccelerationSettings { get; set; }

    [CliOption("--billing-tags-source")]
    public string? BillingTagsSource { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--hop-destinations")]
    public string[]? HopDestinations { get; set; }

    [CliOption("--job-template")]
    public string? JobTemplate { get; set; }

    [CliOption("--priority")]
    public int? Priority { get; set; }

    [CliOption("--queue")]
    public string? Queue { get; set; }

    [CliOption("--simulate-reserved-queue")]
    public string? SimulateReservedQueue { get; set; }

    [CliOption("--status-update-interval")]
    public string? StatusUpdateInterval { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--user-metadata")]
    public IEnumerable<KeyValue>? UserMetadata { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
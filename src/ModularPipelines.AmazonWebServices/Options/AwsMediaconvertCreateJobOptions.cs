using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconvert", "create-job")]
public record AwsMediaconvertCreateJobOptions(
[property: CommandSwitch("--role")] string Role,
[property: CommandSwitch("--settings")] string Settings
) : AwsOptions
{
    [CommandSwitch("--acceleration-settings")]
    public string? AccelerationSettings { get; set; }

    [CommandSwitch("--billing-tags-source")]
    public string? BillingTagsSource { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--hop-destinations")]
    public string[]? HopDestinations { get; set; }

    [CommandSwitch("--job-template")]
    public string? JobTemplate { get; set; }

    [CommandSwitch("--priority")]
    public int? Priority { get; set; }

    [CommandSwitch("--queue")]
    public string? Queue { get; set; }

    [CommandSwitch("--simulate-reserved-queue")]
    public string? SimulateReservedQueue { get; set; }

    [CommandSwitch("--status-update-interval")]
    public string? StatusUpdateInterval { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--user-metadata")]
    public IEnumerable<KeyValue>? UserMetadata { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
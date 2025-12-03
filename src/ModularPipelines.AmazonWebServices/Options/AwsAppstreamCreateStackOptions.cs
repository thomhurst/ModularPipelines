using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "create-stack")]
public record AwsAppstreamCreateStackOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--storage-connectors")]
    public string[]? StorageConnectors { get; set; }

    [CliOption("--redirect-url")]
    public string? RedirectUrl { get; set; }

    [CliOption("--feedback-url")]
    public string? FeedbackUrl { get; set; }

    [CliOption("--user-settings")]
    public string[]? UserSettings { get; set; }

    [CliOption("--application-settings")]
    public string? ApplicationSettings { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--access-endpoints")]
    public string[]? AccessEndpoints { get; set; }

    [CliOption("--embed-host-domains")]
    public string[]? EmbedHostDomains { get; set; }

    [CliOption("--streaming-experience-settings")]
    public string? StreamingExperienceSettings { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
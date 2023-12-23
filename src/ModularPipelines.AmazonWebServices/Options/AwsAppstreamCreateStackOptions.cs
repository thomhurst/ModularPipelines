using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "create-stack")]
public record AwsAppstreamCreateStackOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--storage-connectors")]
    public string[]? StorageConnectors { get; set; }

    [CommandSwitch("--redirect-url")]
    public string? RedirectUrl { get; set; }

    [CommandSwitch("--feedback-url")]
    public string? FeedbackUrl { get; set; }

    [CommandSwitch("--user-settings")]
    public string[]? UserSettings { get; set; }

    [CommandSwitch("--application-settings")]
    public string? ApplicationSettings { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--access-endpoints")]
    public string[]? AccessEndpoints { get; set; }

    [CommandSwitch("--embed-host-domains")]
    public string[]? EmbedHostDomains { get; set; }

    [CommandSwitch("--streaming-experience-settings")]
    public string? StreamingExperienceSettings { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
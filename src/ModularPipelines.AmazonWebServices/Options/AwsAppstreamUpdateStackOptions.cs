using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "update-stack")]
public record AwsAppstreamUpdateStackOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--storage-connectors")]
    public string[]? StorageConnectors { get; set; }

    [CommandSwitch("--redirect-url")]
    public string? RedirectUrl { get; set; }

    [CommandSwitch("--feedback-url")]
    public string? FeedbackUrl { get; set; }

    [CommandSwitch("--attributes-to-delete")]
    public string[]? AttributesToDelete { get; set; }

    [CommandSwitch("--user-settings")]
    public string[]? UserSettings { get; set; }

    [CommandSwitch("--application-settings")]
    public string? ApplicationSettings { get; set; }

    [CommandSwitch("--access-endpoints")]
    public string[]? AccessEndpoints { get; set; }

    [CommandSwitch("--embed-host-domains")]
    public string[]? EmbedHostDomains { get; set; }

    [CommandSwitch("--streaming-experience-settings")]
    public string? StreamingExperienceSettings { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
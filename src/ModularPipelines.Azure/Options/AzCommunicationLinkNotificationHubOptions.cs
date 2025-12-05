using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("communication", "link-notification-hub")]
public record AzCommunicationLinkNotificationHubOptions : AzOptions
{
    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-id")]
    public string? ResourceId { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}
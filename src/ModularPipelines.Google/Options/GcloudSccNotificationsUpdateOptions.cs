using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scc", "notifications", "update")]
public record GcloudSccNotificationsUpdateOptions(
[property: CliArgument] string NotificationConfigId
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--pubsub-topic")]
    public string? PubsubTopic { get; set; }

    [CliOption("--folder")]
    public string? Folder { get; set; }

    [CliOption("--organization")]
    public string? Organization { get; set; }

    [CliOption("--project")]
    public new string? Project { get; set; }
}
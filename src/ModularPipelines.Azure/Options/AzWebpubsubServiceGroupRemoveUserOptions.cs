using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("webpubsub", "service", "group", "remove-user")]
public record AzWebpubsubServiceGroupRemoveUserOptions(
[property: CliOption("--hub-name")] string HubName,
[property: CliOption("--user-id")] string UserId
) : AzOptions
{
    [CliOption("--group-name")]
    public string? GroupName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}
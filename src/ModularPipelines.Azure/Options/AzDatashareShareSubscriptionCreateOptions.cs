using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datashare", "share-subscription", "create")]
public record AzDatashareShareSubscriptionCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--invitation-id")] string InvitationId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--source-share-location")] string SourceShareLocation
) : AzOptions
{
    [CommandSwitch("--expiration-date")]
    public string? ExpirationDate { get; set; }
}


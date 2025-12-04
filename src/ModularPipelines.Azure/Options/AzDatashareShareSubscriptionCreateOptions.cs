using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datashare", "share-subscription", "create")]
public record AzDatashareShareSubscriptionCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--invitation-id")] string InvitationId,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--source-share-location")] string SourceShareLocation
) : AzOptions
{
    [CliOption("--expiration-date")]
    public string? ExpirationDate { get; set; }
}
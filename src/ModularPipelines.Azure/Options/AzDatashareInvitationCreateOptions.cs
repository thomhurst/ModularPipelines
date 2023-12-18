using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datashare", "invitation", "create")]
public record AzDatashareInvitationCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--invitation-name")] string InvitationName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--share-name")] string ShareName
) : AzOptions
{
    [CommandSwitch("--expiration-date")]
    public string? ExpirationDate { get; set; }

    [CommandSwitch("--target-active-directory-id")]
    public string? TargetActiveDirectoryId { get; set; }

    [CommandSwitch("--target-email")]
    public string? TargetEmail { get; set; }

    [CommandSwitch("--target-object-id")]
    public string? TargetObjectId { get; set; }
}
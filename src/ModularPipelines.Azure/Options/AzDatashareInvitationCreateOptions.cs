using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datashare", "invitation", "create")]
public record AzDatashareInvitationCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--invitation-name")] string InvitationName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--share-name")] string ShareName
) : AzOptions
{
    [CliOption("--expiration-date")]
    public string? ExpirationDate { get; set; }

    [CliOption("--target-active-directory-id")]
    public string? TargetActiveDirectoryId { get; set; }

    [CliOption("--target-email")]
    public string? TargetEmail { get; set; }

    [CliOption("--target-object-id")]
    public string? TargetObjectId { get; set; }
}
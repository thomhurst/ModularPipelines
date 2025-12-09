using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datashare", "invitation", "list")]
public record AzDatashareInvitationListOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--share-name")] string ShareName
) : AzOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--orderby")]
    public string? Orderby { get; set; }

    [CliOption("--skip-token")]
    public string? SkipToken { get; set; }
}
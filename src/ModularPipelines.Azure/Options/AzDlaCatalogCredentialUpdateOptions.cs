using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dla", "catalog", "credential", "update")]
public record AzDlaCatalogCredentialUpdateOptions(
[property: CliOption("--credential-name")] string CredentialName,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--uri")] string Uri,
[property: CliOption("--user-name")] string UserName
) : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--new-password")]
    public string? NewPassword { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}
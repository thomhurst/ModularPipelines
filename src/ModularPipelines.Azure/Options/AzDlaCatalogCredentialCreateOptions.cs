using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dla", "catalog", "credential", "create")]
public record AzDlaCatalogCredentialCreateOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--credential-name")] string CredentialName,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--uri")] string Uri,
[property: CliOption("--user-name")] string UserName
) : AzOptions
{
    [CliOption("--password")]
    public string? Password { get; set; }
}
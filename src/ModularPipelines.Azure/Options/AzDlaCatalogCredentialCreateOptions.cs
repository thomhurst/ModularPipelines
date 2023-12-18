using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dla", "catalog", "credential", "create")]
public record AzDlaCatalogCredentialCreateOptions(
[property: CommandSwitch("--account")] int Account,
[property: CommandSwitch("--credential-name")] string CredentialName,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--uri")] string Uri,
[property: CommandSwitch("--user-name")] string UserName
) : AzOptions
{
    [CommandSwitch("--password")]
    public string? Password { get; set; }
}


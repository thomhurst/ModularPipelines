using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dla", "catalog", "credential", "update")]
public record AzDlaCatalogCredentialUpdateOptions(
[property: CommandSwitch("--credential-name")] string CredentialName,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--uri")] string Uri,
[property: CommandSwitch("--user-name")] string UserName
) : AzOptions
{
    [CommandSwitch("--account")]
    public int? Account { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--new-password")]
    public string? NewPassword { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}
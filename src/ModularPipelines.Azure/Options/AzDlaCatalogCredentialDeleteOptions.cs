using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dla", "catalog", "credential", "delete")]
public record AzDlaCatalogCredentialDeleteOptions(
[property: CommandSwitch("--credential-name")] string CredentialName,
[property: CommandSwitch("--database-name")] string DatabaseName
) : AzOptions
{
    [CommandSwitch("--account")]
    public int? Account { get; set; }

    [BooleanCommandSwitch("--cascade")]
    public bool? Cascade { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}
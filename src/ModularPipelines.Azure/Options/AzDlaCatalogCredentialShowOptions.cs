using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dla", "catalog", "credential", "show")]
public record AzDlaCatalogCredentialShowOptions(
[property: CommandSwitch("--credential-name")] string CredentialName,
[property: CommandSwitch("--database-name")] string DatabaseName
) : AzOptions
{
    [CommandSwitch("--account")]
    public int? Account { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "vm", "enable-ure-ad-auth")]
public record AzSqlVmEnableAzureAdAuthOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--msi-client-id")]
    public string? MsiClientId { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--skip-client-validation")]
    public string? SkipClientValidation { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}


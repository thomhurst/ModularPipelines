using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "endpoint", "create", "servicebus")]
public record AzDtEndpointCreateServicebusOptions(
[property: CommandSwitch("--dt-name")] string DtName,
[property: CommandSwitch("--en")] string En,
[property: CommandSwitch("--sbn")] string Sbn,
[property: CommandSwitch("--sbt")] string Sbt
) : AzOptions
{
    [CommandSwitch("--deadletter-sas-uri")]
    public string? DeadletterSasUri { get; set; }

    [CommandSwitch("--deadletter-uri")]
    public string? DeadletterUri { get; set; }

    [BooleanCommandSwitch("--mi-system-assigned")]
    public bool? MiSystemAssigned { get; set; }

    [CommandSwitch("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--sbg")]
    public string? Sbg { get; set; }

    [CommandSwitch("--sbp")]
    public string? Sbp { get; set; }

    [CommandSwitch("--sbs")]
    public string? Sbs { get; set; }
}
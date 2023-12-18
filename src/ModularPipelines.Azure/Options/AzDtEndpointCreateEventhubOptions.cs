using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "endpoint", "create", "eventhub")]
public record AzDtEndpointCreateEventhubOptions(
[property: CommandSwitch("--dt-name")] string DtName,
[property: CommandSwitch("--eh")] string Eh,
[property: CommandSwitch("--ehn")] string Ehn,
[property: CommandSwitch("--en")] string En
) : AzOptions
{
    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [CommandSwitch("--deadletter-sas-uri")]
    public string? DeadletterSasUri { get; set; }

    [CommandSwitch("--deadletter-uri")]
    public string? DeadletterUri { get; set; }

    [CommandSwitch("--ehg")]
    public string? Ehg { get; set; }

    [CommandSwitch("--ehp")]
    public string? Ehp { get; set; }

    [CommandSwitch("--ehs")]
    public string? Ehs { get; set; }

    [BooleanCommandSwitch("--mi-system-assigned")]
    public bool? MiSystemAssigned { get; set; }

    [CommandSwitch("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}
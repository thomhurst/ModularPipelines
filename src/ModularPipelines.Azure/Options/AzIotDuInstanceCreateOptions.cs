using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "du", "instance", "create")]
public record AzIotDuInstanceCreateOptions(
[property: CommandSwitch("--account")] int Account,
[property: CommandSwitch("--instance")] string Instance,
[property: CommandSwitch("--iothub-ids")] string IothubIds
) : AzOptions
{
    [CommandSwitch("--diagnostics-storage-id")]
    public string? DiagnosticsStorageId { get; set; }

    [BooleanCommandSwitch("--enable-diagnostics")]
    public bool? EnableDiagnostics { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}


using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "diagnostics", "validate-messages")]
public record AzIotCentralDiagnosticsValidateMessagesOptions(
[property: CommandSwitch("--app-id")] string AppId
) : AzOptions
{
    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--cg")]
    public string? Cg { get; set; }

    [CommandSwitch("--device-id")]
    public string? DeviceId { get; set; }

    [CommandSwitch("--dr")]
    public string? Dr { get; set; }

    [CommandSwitch("--enqueued-time")]
    public string? EnqueuedTime { get; set; }

    [CommandSwitch("--max-messages")]
    public string? MaxMessages { get; set; }

    [CommandSwitch("--minimum-severity")]
    public string? MinimumSeverity { get; set; }

    [CommandSwitch("--module-id")]
    public string? ModuleId { get; set; }

    [CommandSwitch("--properties")]
    public string? Properties { get; set; }

    [BooleanCommandSwitch("--repair")]
    public bool? Repair { get; set; }

    [CommandSwitch("--style")]
    public string? Style { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}
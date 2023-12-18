using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "script-execution", "create")]
public record AzVmwareScriptExecutionCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--private-cloud")] string PrivateCloud,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--timeout")] string Timeout
) : AzOptions
{
    [CommandSwitch("--failure-reason")]
    public string? FailureReason { get; set; }

    [CommandSwitch("--hidden-parameter")]
    public string? HiddenParameter { get; set; }

    [CommandSwitch("--named-outputs")]
    public string? NamedOutputs { get; set; }

    [CommandSwitch("--out")]
    public string? Out { get; set; }

    [CommandSwitch("--parameter")]
    public string? Parameter { get; set; }

    [CommandSwitch("--retention")]
    public string? Retention { get; set; }

    [CommandSwitch("--script-cmdlet-id")]
    public string? ScriptCmdletId { get; set; }
}
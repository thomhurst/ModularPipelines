using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmware", "script-execution", "create")]
public record AzVmwareScriptExecutionCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--private-cloud")] string PrivateCloud,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--timeout")] string Timeout
) : AzOptions
{
    [CliOption("--failure-reason")]
    public string? FailureReason { get; set; }

    [CliOption("--hidden-parameter")]
    public string? HiddenParameter { get; set; }

    [CliOption("--named-outputs")]
    public string? NamedOutputs { get; set; }

    [CliOption("--out")]
    public string? Out { get; set; }

    [CliOption("--parameter")]
    public string? Parameter { get; set; }

    [CliOption("--retention")]
    public string? Retention { get; set; }

    [CliOption("--script-cmdlet-id")]
    public string? ScriptCmdletId { get; set; }
}
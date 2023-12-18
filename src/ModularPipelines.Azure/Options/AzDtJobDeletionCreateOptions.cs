using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "job", "deletion", "create")]
public record AzDtJobDeletionCreateOptions(
[property: CommandSwitch("--dt-name")] string DtName
) : AzOptions
{
    [CommandSwitch("--job-id")]
    public string? JobId { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}


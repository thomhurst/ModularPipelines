using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "job", "deletion", "show")]
public record AzDtJobDeletionShowOptions(
[property: CommandSwitch("--dt-name")] string DtName,
[property: CommandSwitch("--job-id")] string JobId
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batchai", "job", "file", "list")]
public record AzBatchaiJobFileListOptions(
[property: CommandSwitch("--experiment")] string Experiment,
[property: CommandSwitch("--job")] string Job,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace")] string Workspace
) : AzOptions
{
    [CommandSwitch("--expiry")]
    public string? Expiry { get; set; }

    [CommandSwitch("--output-directory-id")]
    public string? OutputDirectoryId { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }
}


using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batchai", "job", "file", "stream")]
public record AzBatchaiJobFileStreamOptions(
[property: CommandSwitch("--experiment")] string Experiment,
[property: CommandSwitch("--file-name")] string FileName,
[property: CommandSwitch("--job")] string Job,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace")] string Workspace
) : AzOptions
{
    [CommandSwitch("--output-directory-id")]
    public string? OutputDirectoryId { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }
}


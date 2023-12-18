using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "job", "create")]
public record AzMlJobCreateOptions(
[property: CommandSwitch("--file")] string File,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--save-as")]
    public string? SaveAs { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [BooleanCommandSwitch("--skip-validation")]
    public bool? SkipValidation { get; set; }

    [BooleanCommandSwitch("--stream")]
    public bool? Stream { get; set; }

    [BooleanCommandSwitch("--web")]
    public bool? Web { get; set; }
}
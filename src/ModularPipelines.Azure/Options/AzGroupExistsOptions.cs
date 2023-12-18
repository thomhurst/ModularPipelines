using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("group", "exists")]
public record AzGroupExistsOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [BooleanCommandSwitch("--include-comments")]
    public bool? IncludeComments { get; set; }

    [BooleanCommandSwitch("--include-parameter-default-value")]
    public bool? IncludeParameterDefaultValue { get; set; }

    [CommandSwitch("--resource-ids")]
    public string? ResourceIds { get; set; }

    [BooleanCommandSwitch("--skip-all-params")]
    public bool? SkipAllParams { get; set; }

    [BooleanCommandSwitch("--skip-resource-name-params")]
    public bool? SkipResourceNameParams { get; set; }
}
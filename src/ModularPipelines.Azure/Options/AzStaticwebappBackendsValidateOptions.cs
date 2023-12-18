using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("staticwebapp", "backends", "validate")]
public record AzStaticwebappBackendsValidateOptions(
[property: CommandSwitch("--backend-resource-id")] string BackendResourceId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--backend-region")]
    public string? BackendRegion { get; set; }

    [CommandSwitch("--environment-name")]
    public string? EnvironmentName { get; set; }
}


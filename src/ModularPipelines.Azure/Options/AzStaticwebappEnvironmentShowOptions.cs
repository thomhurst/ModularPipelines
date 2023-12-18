using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("staticwebapp", "environment", "show")]
public record AzStaticwebappEnvironmentShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}
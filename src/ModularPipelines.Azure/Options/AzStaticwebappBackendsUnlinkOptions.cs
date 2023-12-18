using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("staticwebapp", "backends", "unlink")]
public record AzStaticwebappBackendsUnlinkOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--environment-name")]
    public string? EnvironmentName { get; set; }

    [BooleanCommandSwitch("--remove-backend-auth")]
    public bool? RemoveBackendAuth { get; set; }
}
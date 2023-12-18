using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devcenter", "admin", "environment-type", "create")]
public record AzDevcenterAdminEnvironmentTypeCreateOptions(
[property: CommandSwitch("--dev-center")] string DevCenter,
[property: CommandSwitch("--environment-type-name")] string EnvironmentTypeName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}
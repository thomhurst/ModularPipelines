using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mesh", "secretvalue", "show")]
public record AzMeshSecretvalueShowOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--secret-name")] string SecretName,
[property: CommandSwitch("--version")] string Version
) : AzOptions
{
    [BooleanCommandSwitch("--show-value")]
    public bool? ShowValue { get; set; }
}
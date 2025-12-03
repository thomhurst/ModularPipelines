using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mesh", "secretvalue", "show")]
public record AzMeshSecretvalueShowOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--secret-name")] string SecretName,
[property: CliOption("--version")] string Version
) : AzOptions
{
    [CliFlag("--show-value")]
    public bool? ShowValue { get; set; }
}
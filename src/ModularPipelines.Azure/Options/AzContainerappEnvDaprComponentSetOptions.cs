using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "env", "dapr-component", "set")]
public record AzContainerappEnvDaprComponentSetOptions(
[property: CliOption("--dapr-component-name")] string DaprComponentName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--yaml")] string Yaml
) : AzOptions;
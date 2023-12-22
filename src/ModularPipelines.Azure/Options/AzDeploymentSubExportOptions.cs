using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment", "sub", "export")]
public record AzDeploymentSubExportOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;
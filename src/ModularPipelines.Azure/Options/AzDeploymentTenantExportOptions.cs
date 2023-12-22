using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment", "tenant", "export")]
public record AzDeploymentTenantExportOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory", "integration-runtime", "list-auth-key")]
public record AzDatafactoryIntegrationRuntimeListAuthKeyOptions(
[property: CommandSwitch("--factory-name")] string FactoryName,
[property: CommandSwitch("--integration-runtime-name")] string IntegrationRuntimeName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;
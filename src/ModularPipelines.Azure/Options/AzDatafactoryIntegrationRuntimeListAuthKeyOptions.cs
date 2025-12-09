using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datafactory", "integration-runtime", "list-auth-key")]
public record AzDatafactoryIntegrationRuntimeListAuthKeyOptions(
[property: CliOption("--factory-name")] string FactoryName,
[property: CliOption("--integration-runtime-name")] string IntegrationRuntimeName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;
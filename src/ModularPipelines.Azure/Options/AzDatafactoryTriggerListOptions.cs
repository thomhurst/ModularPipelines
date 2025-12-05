using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datafactory", "trigger", "list")]
public record AzDatafactoryTriggerListOptions(
[property: CliOption("--factory-name")] string FactoryName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;
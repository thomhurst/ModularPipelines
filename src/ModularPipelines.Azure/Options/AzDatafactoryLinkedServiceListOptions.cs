using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datafactory", "linked-service", "list")]
public record AzDatafactoryLinkedServiceListOptions(
[property: CliOption("--factory-name")] string FactoryName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;
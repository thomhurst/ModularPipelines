using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory", "linked-service", "list")]
public record AzDatafactoryLinkedServiceListOptions(
[property: CommandSwitch("--factory-name")] string FactoryName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;
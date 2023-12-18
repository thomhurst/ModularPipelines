using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tsi", "reference-data-set", "list")]
public record AzTsiReferenceDataSetListOptions(
[property: CommandSwitch("--environment-name")] string EnvironmentName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;
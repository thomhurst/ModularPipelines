using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("csvmware", "resource-pool", "list")]
public record AzCsvmwareResourcePoolListOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--private-cloud")] string PrivateCloud
) : AzOptions;
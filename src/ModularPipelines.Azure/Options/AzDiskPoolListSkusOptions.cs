using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("disk-pool", "list-skus")]
public record AzDiskPoolListSkusOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;
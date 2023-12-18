using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("csvmware", "private-cloud", "list")]
public record AzCsvmwarePrivateCloudListOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastream", "operations", "list")]
public record GcloudDatastreamOperationsListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;
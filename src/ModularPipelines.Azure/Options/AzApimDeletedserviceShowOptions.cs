using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim", "deletedservice", "show")]
public record AzApimDeletedserviceShowOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--service-name")] string ServiceName
) : AzOptions;
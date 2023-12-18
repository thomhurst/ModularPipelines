using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim", "deletedservice", "purge")]
public record AzApimDeletedservicePurgeOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--service-name")] string ServiceName
) : AzOptions
{
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "mesh", "get-revisions")]
public record AzAksMeshGetRevisionsOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
}
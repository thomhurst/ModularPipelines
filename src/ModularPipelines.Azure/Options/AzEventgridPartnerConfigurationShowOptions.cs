using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "configuration", "show")]
public record AzEventgridPartnerConfigurationShowOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}
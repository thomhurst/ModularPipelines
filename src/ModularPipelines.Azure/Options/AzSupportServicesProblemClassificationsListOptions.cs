using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support", "services", "problem-classifications", "list")]
public record AzSupportServicesProblemClassificationsListOptions(
[property: CommandSwitch("--service-name")] string ServiceName
) : AzOptions
{
}


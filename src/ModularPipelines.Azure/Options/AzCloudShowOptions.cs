using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloud", "show")]
public record AzCloudShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}


using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "config", "soft-delete", "show")]
public record AzAcrConfigSoftDeleteShowOptions(
[property: CommandSwitch("--registry")] string Registry
) : AzOptions
{
}
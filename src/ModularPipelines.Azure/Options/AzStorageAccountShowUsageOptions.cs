using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "account", "show-usage")]
public record AzStorageAccountShowUsageOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
}
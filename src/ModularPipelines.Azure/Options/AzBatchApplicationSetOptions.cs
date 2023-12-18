using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "application", "set")]
public record AzBatchApplicationSetOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--allow-updates")]
    public bool? AllowUpdates { get; set; }

    [CommandSwitch("--default-version")]
    public string? DefaultVersion { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }
}
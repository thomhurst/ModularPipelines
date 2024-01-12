using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "operations", "list")]
public record GcloudStorageOperationsListOptions(
[property: PositionalArgument] string ParentResourceName
) : GcloudOptions
{
    [CommandSwitch("--server-filter")]
    public string? ServerFilter { get; set; }
}
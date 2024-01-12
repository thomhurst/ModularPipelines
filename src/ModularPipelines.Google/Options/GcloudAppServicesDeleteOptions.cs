using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "services", "delete")]
public record GcloudAppServicesDeleteOptions(
[property: PositionalArgument] string Services
) : GcloudOptions
{
    [CommandSwitch("--version")]
    public new string? Version { get; set; }
}
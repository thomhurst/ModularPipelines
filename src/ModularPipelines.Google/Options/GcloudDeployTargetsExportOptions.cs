using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "targets", "export")]
public record GcloudDeployTargetsExportOptions(
[property: PositionalArgument] string Target,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [CommandSwitch("--destination")]
    public string? Destination { get; set; }
}
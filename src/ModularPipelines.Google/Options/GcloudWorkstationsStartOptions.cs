using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workstations", "start")]
public record GcloudWorkstationsStartOptions(
[property: PositionalArgument] string Workstation,
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Config,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}
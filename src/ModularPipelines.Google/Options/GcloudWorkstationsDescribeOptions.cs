using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workstations", "describe")]
public record GcloudWorkstationsDescribeOptions(
[property: CliArgument] string Workstation,
[property: CliArgument] string Cluster,
[property: CliArgument] string Config,
[property: CliArgument] string Region
) : GcloudOptions;
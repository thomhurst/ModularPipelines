using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workstations", "configs", "describe")]
public record GcloudWorkstationsConfigsDescribeOptions(
[property: CliArgument] string Config,
[property: CliArgument] string Cluster,
[property: CliArgument] string Region
) : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "domain-mappings", "describe")]
public record GcloudAppDomainMappingsDescribeOptions(
[property: CliArgument] string Domain
) : GcloudOptions;
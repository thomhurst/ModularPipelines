using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "domain-mappings", "delete")]
public record GcloudAppDomainMappingsDeleteOptions(
[property: CliArgument] string Domain
) : GcloudOptions;
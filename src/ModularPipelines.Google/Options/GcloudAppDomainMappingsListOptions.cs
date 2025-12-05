using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "domain-mappings", "list")]
public record GcloudAppDomainMappingsListOptions : GcloudOptions;
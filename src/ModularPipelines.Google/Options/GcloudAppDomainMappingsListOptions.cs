using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "domain-mappings", "list")]
public record GcloudAppDomainMappingsListOptions : GcloudOptions;
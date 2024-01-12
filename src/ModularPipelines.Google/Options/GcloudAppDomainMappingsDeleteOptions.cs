using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "domain-mappings", "delete")]
public record GcloudAppDomainMappingsDeleteOptions(
[property: PositionalArgument] string Domain
) : GcloudOptions;
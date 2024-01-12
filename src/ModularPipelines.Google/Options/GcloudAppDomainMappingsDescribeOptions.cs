using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "domain-mappings", "describe")]
public record GcloudAppDomainMappingsDescribeOptions(
[property: PositionalArgument] string Domain
) : GcloudOptions;
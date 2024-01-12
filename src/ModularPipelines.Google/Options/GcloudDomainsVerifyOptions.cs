using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("domains", "verify")]
public record GcloudDomainsVerifyOptions(
[property: PositionalArgument] string Domain
) : GcloudOptions;
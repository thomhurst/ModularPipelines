using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("domains", "verify")]
public record GcloudDomainsVerifyOptions(
[property: CliArgument] string Domain
) : GcloudOptions;
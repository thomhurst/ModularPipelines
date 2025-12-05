using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "policies", "delete")]
public record GcloudDnsPoliciesDeleteOptions(
[property: CliArgument] string Policy
) : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "policies", "delete")]
public record GcloudAccessContextManagerPoliciesDeleteOptions(
[property: CliArgument] string Policy
) : GcloudOptions;
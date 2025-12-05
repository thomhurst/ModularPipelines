using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "policies", "list")]
public record GcloudAccessContextManagerPoliciesListOptions(
[property: CliOption("--organization")] string Organization
) : GcloudOptions;
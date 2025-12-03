using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tsi", "access-policy", "list")]
public record AzTsiAccessPolicyListOptions(
[property: CliOption("--environment-name")] string EnvironmentName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;
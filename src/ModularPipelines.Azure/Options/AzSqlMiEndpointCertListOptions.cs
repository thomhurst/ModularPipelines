using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "mi", "endpoint-cert", "list")]
public record AzSqlMiEndpointCertListOptions(
[property: CliOption("--instance-name")] string InstanceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("postgres", "flexible-server", "identity", "list")]
public record AzPostgresFlexibleServerIdentityListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--server-name")] string ServerName
) : AzOptions;
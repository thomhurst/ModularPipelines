using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "app", "identity", "force-set")]
public record AzSpringAppIdentityForceSetOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service,
[property: CliOption("--system-assigned")] string SystemAssigned,
[property: CliOption("--user-assigned")] string UserAssigned
) : AzOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "trustedaccess", "role", "list")]
public record AzAksTrustedaccessRoleListOptions(
[property: CliOption("--location")] string Location
) : AzOptions;
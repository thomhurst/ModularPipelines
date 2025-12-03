using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ad", "app", "permission", "admin-consent")]
public record AzAdAppPermissionAdminConsentOptions(
[property: CliOption("--id")] string Id
) : AzOptions;
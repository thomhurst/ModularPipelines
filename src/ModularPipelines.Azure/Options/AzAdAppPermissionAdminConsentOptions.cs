using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "app", "permission", "admin-consent")]
public record AzAdAppPermissionAdminConsentOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iap", "oauth-brands", "create")]
public record GcloudIapOauthBrandsCreateOptions(
[property: CommandSwitch("--application_title")] string ApplicationTitle,
[property: CommandSwitch("--support_email")] string SupportEmail
) : GcloudOptions;
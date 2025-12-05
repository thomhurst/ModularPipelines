using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iap", "oauth-brands", "create")]
public record GcloudIapOauthBrandsCreateOptions(
[property: CliOption("--application_title")] string ApplicationTitle,
[property: CliOption("--support_email")] string SupportEmail
) : GcloudOptions;
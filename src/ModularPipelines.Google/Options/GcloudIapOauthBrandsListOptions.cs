using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iap", "oauth-brands", "list")]
public record GcloudIapOauthBrandsListOptions : GcloudOptions;
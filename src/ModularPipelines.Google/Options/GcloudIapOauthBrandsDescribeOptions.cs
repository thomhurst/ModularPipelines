using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iap", "oauth-brands", "describe")]
public record GcloudIapOauthBrandsDescribeOptions(
[property: CliArgument] string Name
) : GcloudOptions;
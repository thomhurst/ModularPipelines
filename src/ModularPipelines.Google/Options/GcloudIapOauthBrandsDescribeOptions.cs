using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iap", "oauth-brands", "describe")]
public record GcloudIapOauthBrandsDescribeOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;
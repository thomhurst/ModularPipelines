using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("recaptcha", "keys", "describe")]
public record GcloudRecaptchaKeysDescribeOptions(
[property: CliArgument] string Key
) : GcloudOptions;
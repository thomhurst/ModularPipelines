using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("recaptcha", "keys", "delete")]
public record GcloudRecaptchaKeysDeleteOptions(
[property: CliArgument] string Key
) : GcloudOptions;
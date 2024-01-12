using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("recaptcha", "keys", "delete")]
public record GcloudRecaptchaKeysDeleteOptions(
[property: PositionalArgument] string Key
) : GcloudOptions;
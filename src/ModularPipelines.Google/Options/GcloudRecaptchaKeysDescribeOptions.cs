using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("recaptcha", "keys", "describe")]
public record GcloudRecaptchaKeysDescribeOptions(
[property: PositionalArgument] string Key
) : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("recaptcha", "keys", "list")]
public record GcloudRecaptchaKeysListOptions : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("recaptcha", "keys", "migrate")]
public record GcloudRecaptchaKeysMigrateOptions(
[property: PositionalArgument] string Key
) : GcloudOptions
{
    [BooleanCommandSwitch("--skip-billing-check")]
    public bool? SkipBillingCheck { get; set; }
}
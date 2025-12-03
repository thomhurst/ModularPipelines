using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("recaptcha", "keys", "migrate")]
public record GcloudRecaptchaKeysMigrateOptions(
[property: CliArgument] string Key
) : GcloudOptions
{
    [CliFlag("--skip-billing-check")]
    public bool? SkipBillingCheck { get; set; }
}
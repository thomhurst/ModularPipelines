using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firebase", "test", "android", "locales", "describe")]
public record GcloudFirebaseTestAndroidLocalesDescribeOptions(
[property: CliArgument] string Locale
) : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firebase", "test", "ios", "locales", "describe")]
public record GcloudFirebaseTestIosLocalesDescribeOptions(
[property: CliArgument] string Locale
) : GcloudOptions;
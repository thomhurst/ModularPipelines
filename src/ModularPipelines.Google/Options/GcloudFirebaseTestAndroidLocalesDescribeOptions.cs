using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firebase", "test", "android", "locales", "describe")]
public record GcloudFirebaseTestAndroidLocalesDescribeOptions(
[property: PositionalArgument] string Locale
) : GcloudOptions;
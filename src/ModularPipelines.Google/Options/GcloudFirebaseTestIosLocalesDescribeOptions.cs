using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firebase", "test", "ios", "locales", "describe")]
public record GcloudFirebaseTestIosLocalesDescribeOptions(
[property: PositionalArgument] string Locale
) : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firebase", "test", "android", "versions", "describe")]
public record GcloudFirebaseTestAndroidVersionsDescribeOptions(
[property: PositionalArgument] string VersionId
) : GcloudOptions;
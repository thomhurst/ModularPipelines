using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firebase", "test", "ios", "versions", "describe")]
public record GcloudFirebaseTestIosVersionsDescribeOptions(
[property: PositionalArgument] string VersionId
) : GcloudOptions;
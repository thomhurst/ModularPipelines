using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firebase", "test", "ios", "versions", "describe")]
public record GcloudFirebaseTestIosVersionsDescribeOptions(
[property: CliArgument] string VersionId
) : GcloudOptions;
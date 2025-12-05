using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firebase", "test", "android", "versions", "describe")]
public record GcloudFirebaseTestAndroidVersionsDescribeOptions(
[property: CliArgument] string VersionId
) : GcloudOptions;
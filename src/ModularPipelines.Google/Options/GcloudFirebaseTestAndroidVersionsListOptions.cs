using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firebase", "test", "android", "versions", "list")]
public record GcloudFirebaseTestAndroidVersionsListOptions : GcloudOptions;
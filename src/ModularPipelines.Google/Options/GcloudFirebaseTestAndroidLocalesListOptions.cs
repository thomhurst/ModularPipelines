using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firebase", "test", "android", "locales", "list")]
public record GcloudFirebaseTestAndroidLocalesListOptions : GcloudOptions;
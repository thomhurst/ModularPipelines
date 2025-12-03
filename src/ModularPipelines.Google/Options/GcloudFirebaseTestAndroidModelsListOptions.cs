using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firebase", "test", "android", "models", "list")]
public record GcloudFirebaseTestAndroidModelsListOptions : GcloudOptions;
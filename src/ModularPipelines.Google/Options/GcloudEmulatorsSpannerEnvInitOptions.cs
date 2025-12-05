using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emulators", "spanner", "env-init")]
public record GcloudEmulatorsSpannerEnvInitOptions : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emulators", "spanner", "env-init")]
public record GcloudEmulatorsSpannerEnvInitOptions : GcloudOptions;
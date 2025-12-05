using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firebase", "test", "ios", "locales", "list")]
public record GcloudFirebaseTestIosLocalesListOptions : GcloudOptions;
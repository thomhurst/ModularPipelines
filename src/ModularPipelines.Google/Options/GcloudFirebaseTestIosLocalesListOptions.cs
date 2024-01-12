using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firebase", "test", "ios", "locales", "list")]
public record GcloudFirebaseTestIosLocalesListOptions : GcloudOptions;
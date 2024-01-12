using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firebase", "test", "ios", "versions", "list")]
public record GcloudFirebaseTestIosVersionsListOptions : GcloudOptions;
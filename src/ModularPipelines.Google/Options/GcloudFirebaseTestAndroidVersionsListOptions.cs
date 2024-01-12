using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firebase", "test", "android", "versions", "list")]
public record GcloudFirebaseTestAndroidVersionsListOptions : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firebase", "test", "network-profiles", "list")]
public record GcloudFirebaseTestNetworkProfilesListOptions : GcloudOptions;
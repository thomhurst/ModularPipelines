using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firebase", "test", "network-profiles", "list")]
public record GcloudFirebaseTestNetworkProfilesListOptions : GcloudOptions;
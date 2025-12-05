using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firebase", "test", "network-profiles", "describe")]
public record GcloudFirebaseTestNetworkProfilesDescribeOptions(
[property: CliArgument] string ProfileId
) : GcloudOptions;
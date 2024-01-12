using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firebase", "test", "network-profiles", "describe")]
public record GcloudFirebaseTestNetworkProfilesDescribeOptions(
[property: PositionalArgument] string ProfileId
) : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("identity", "groups", "describe")]
public record GcloudIdentityGroupsDescribeOptions(
[property: CliArgument] string Email
) : GcloudOptions;
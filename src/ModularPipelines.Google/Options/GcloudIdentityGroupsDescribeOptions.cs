using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("identity", "groups", "describe")]
public record GcloudIdentityGroupsDescribeOptions(
[property: PositionalArgument] string Email
) : GcloudOptions;
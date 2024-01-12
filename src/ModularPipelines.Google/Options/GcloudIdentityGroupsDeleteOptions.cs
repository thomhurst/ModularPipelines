using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("identity", "groups", "delete")]
public record GcloudIdentityGroupsDeleteOptions(
[property: PositionalArgument] string Email
) : GcloudOptions;
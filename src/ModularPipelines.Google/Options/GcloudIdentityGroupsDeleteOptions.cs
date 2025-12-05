using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("identity", "groups", "delete")]
public record GcloudIdentityGroupsDeleteOptions(
[property: CliArgument] string Email
) : GcloudOptions;
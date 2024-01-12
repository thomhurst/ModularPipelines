using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auth", "revoke")]
public record GcloudAuthRevokeOptions(
[property: PositionalArgument] string Accounts
) : GcloudOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }
}
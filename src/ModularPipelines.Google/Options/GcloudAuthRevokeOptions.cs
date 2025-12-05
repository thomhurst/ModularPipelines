using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auth", "revoke")]
public record GcloudAuthRevokeOptions(
[property: CliArgument] string Accounts
) : GcloudOptions
{
    [CliFlag("--all")]
    public bool? All { get; set; }
}
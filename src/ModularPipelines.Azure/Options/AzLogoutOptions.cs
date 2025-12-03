using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logout")]
public record AzLogoutOptions : AzOptions
{
    [CliOption("--username")]
    public string? Username { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acr", "login")]
public record AzAcrLoginOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--expose-token")]
    public bool? ExposeToken { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--suffix")]
    public string? Suffix { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("staticwebapp", "reconnect")]
public record AzStaticwebappReconnectOptions(
[property: CliOption("--branch")] string Branch,
[property: CliOption("--name")] string Name,
[property: CliOption("--source")] string Source
) : AzOptions
{
    [CliFlag("--login-with-github")]
    public bool? LoginWithGithub { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}
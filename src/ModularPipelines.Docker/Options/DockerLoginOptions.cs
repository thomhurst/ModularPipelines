using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("login")]
[ExcludeFromCodeCoverage]
public record DockerLoginOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Server { get; set; }

    [CliOption("--password")]
    public virtual string? Password { get; set; }

    [CliOption("--password-stdin")]
    public virtual string? PasswordStdin { get; set; }

    [CliOption("--username")]
    public virtual string? Username { get; set; }
}

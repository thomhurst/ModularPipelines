using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerSecretCreateOptions : DockerOptions
{
    public DockerSecretCreateOptions(
        string secret
    )
    {
        CommandParts = ["secret", "create"];

        Secret = secret;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Secret { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? File { get; set; }

    [CommandSwitch("--driver")]
    public string? Driver { get; set; }

    [CommandSwitch("--label")]
    public string? Label { get; set; }

    [CommandSwitch("--template-driver")]
    public string? TemplateDriver { get; set; }
}

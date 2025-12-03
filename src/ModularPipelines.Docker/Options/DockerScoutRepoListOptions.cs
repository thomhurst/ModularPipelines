using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("scout", "repo", "list")]
[ExcludeFromCodeCoverage]
public record DockerScoutRepoListOptions : DockerOptions
{
    [CliOption("--filter")]
    public virtual string? Filter { get; set; }

    [CliOption("--only-disabled")]
    public virtual string? OnlyDisabled { get; set; }

    [CliOption("--only-enabled")]
    public virtual string? OnlyEnabled { get; set; }

    [CliOption("--only-registry")]
    public virtual string? OnlyRegistry { get; set; }

    [CliOption("--org")]
    public virtual string? Org { get; set; }
}

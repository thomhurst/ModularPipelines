using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spring", "application-accelerator", "customized-accelerator", "update")]
public record AzSpringApplicationAcceleratorCustomizedAcceleratorUpdateOptions(
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--git-url")] string GitUrl,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliOption("--accelerator-tags")]
    public string? AcceleratorTags { get; set; }

    [CliOption("--ca-cert-name")]
    public string? CaCertName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--git-branch")]
    public string? GitBranch { get; set; }

    [CliOption("--git-commit")]
    public string? GitCommit { get; set; }

    [CliOption("--git-interval")]
    public string? GitInterval { get; set; }

    [CliOption("--git-sub-path")]
    public string? GitSubPath { get; set; }

    [CliOption("--git-tag")]
    public string? GitTag { get; set; }

    [CliOption("--host-key")]
    public string? HostKey { get; set; }

    [CliOption("--host-key-algorithm")]
    public string? HostKeyAlgorithm { get; set; }

    [CliOption("--icon-url")]
    public string? IconUrl { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--private-key")]
    public string? PrivateKey { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }
}
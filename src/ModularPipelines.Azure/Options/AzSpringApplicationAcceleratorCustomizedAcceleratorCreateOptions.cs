using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "application-accelerator", "customized-accelerator", "create")]
public record AzSpringApplicationAcceleratorCustomizedAcceleratorCreateOptions(
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--git-url")] string GitUrl,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service
) : AzOptions
{
    [CommandSwitch("--accelerator-tags")]
    public string? AcceleratorTags { get; set; }

    [CommandSwitch("--ca-cert-name")]
    public string? CaCertName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--git-branch")]
    public string? GitBranch { get; set; }

    [CommandSwitch("--git-commit")]
    public string? GitCommit { get; set; }

    [CommandSwitch("--git-interval")]
    public string? GitInterval { get; set; }

    [CommandSwitch("--git-sub-path")]
    public string? GitSubPath { get; set; }

    [CommandSwitch("--git-tag")]
    public string? GitTag { get; set; }

    [CommandSwitch("--host-key")]
    public string? HostKey { get; set; }

    [CommandSwitch("--host-key-algorithm")]
    public string? HostKeyAlgorithm { get; set; }

    [CommandSwitch("--icon-url")]
    public string? IconUrl { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--private-key")]
    public string? PrivateKey { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }
}
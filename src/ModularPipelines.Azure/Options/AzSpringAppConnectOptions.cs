using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spring", "app", "connect")]
public record AzSpringAppConnectOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliOption("--deployment")]
    public string? Deployment { get; set; }

    [CliOption("--instance")]
    public string? Instance { get; set; }

    [CliOption("--shell-cmd")]
    public string? ShellCmd { get; set; }
}
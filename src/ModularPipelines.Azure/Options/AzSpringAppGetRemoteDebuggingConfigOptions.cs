using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "app", "get-remote-debugging-config")]
public record AzSpringAppGetRemoteDebuggingConfigOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliOption("--deployment")]
    public string? Deployment { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("webapp", "deployment", "user", "set")]
public record AzWebappDeploymentUserSetOptions(
[property: CliOption("--user-name")] string UserName
) : AzOptions
{
    [CliOption("--password")]
    public string? Password { get; set; }
}
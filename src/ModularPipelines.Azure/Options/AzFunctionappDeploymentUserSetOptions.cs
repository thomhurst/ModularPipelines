using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("functionapp", "deployment", "user", "set")]
public record AzFunctionappDeploymentUserSetOptions(
[property: CliOption("--user-name")] string UserName
) : AzOptions
{
    [CliOption("--password")]
    public string? Password { get; set; }
}
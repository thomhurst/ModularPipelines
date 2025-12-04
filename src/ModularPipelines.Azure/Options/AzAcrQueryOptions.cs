using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "query")]
public record AzAcrQueryOptions(
[property: CliOption("--kql-query")] string KqlQuery,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--repository")]
    public string? Repository { get; set; }

    [CliOption("--skip-token")]
    public string? SkipToken { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }
}
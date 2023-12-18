using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "query")]
public record AzAcrQueryOptions(
[property: CommandSwitch("--kql-query")] string KqlQuery,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--repository")]
    public string? Repository { get; set; }

    [CommandSwitch("--skip-token")]
    public string? SkipToken { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }
}
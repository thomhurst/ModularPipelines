using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "repository", "update")]
public record AzAcrRepositoryUpdateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--delete-enabled")]
    public bool? DeleteEnabled { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliFlag("--list-enabled")]
    public bool? ListEnabled { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliFlag("--read-enabled")]
    public bool? ReadEnabled { get; set; }

    [CliOption("--repository")]
    public string? Repository { get; set; }

    [CliOption("--suffix")]
    public string? Suffix { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }

    [CliFlag("--write-enabled")]
    public bool? WriteEnabled { get; set; }
}
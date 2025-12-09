using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "repository", "delete")]
public record AzAcrRepositoryDeleteOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--repository")]
    public string? Repository { get; set; }

    [CliOption("--suffix")]
    public string? Suffix { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}
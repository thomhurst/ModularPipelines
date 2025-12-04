using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "repository", "untag")]
public record AzAcrRepositoryUntagOptions(
[property: CliOption("--image")] string Image,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--suffix")]
    public string? Suffix { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acr", "repository", "show-tags")]
public record AzAcrRepositoryShowTagsOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--repository")] string Repository
) : AzOptions
{
    [CliFlag("--detail")]
    public bool? Detail { get; set; }

    [CliOption("--orderby")]
    public string? Orderby { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--suffix")]
    public string? Suffix { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }
}
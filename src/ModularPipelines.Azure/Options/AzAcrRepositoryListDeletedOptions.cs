using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acr", "repository", "list-deleted")]
public record AzAcrRepositoryListDeletedOptions(
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
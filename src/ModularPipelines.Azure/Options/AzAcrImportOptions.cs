using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acr", "import")]
public record AzAcrImportOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--source")] string Source
) : AzOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--registry")]
    public string? Registry { get; set; }

    [CliOption("--repository")]
    public string? Repository { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }
}
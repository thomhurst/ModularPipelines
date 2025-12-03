using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "du", "update", "import")]
public record AzIotDuUpdateImportOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--instance")] string Instance,
[property: CliOption("--url")] string Url
) : AzOptions
{
    [CliOption("--defer")]
    public string? Defer { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

    [CliOption("--friendly-name")]
    public string? FriendlyName { get; set; }

    [CliOption("--hashes")]
    public string? Hashes { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--size")]
    public string? Size { get; set; }
}
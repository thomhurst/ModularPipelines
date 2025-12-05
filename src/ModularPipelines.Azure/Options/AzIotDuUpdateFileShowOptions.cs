using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "du", "update", "file", "show")]
public record AzIotDuUpdateFileShowOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--instance")] string Instance,
[property: CliOption("--ufid")] string Ufid,
[property: CliOption("--un")] string Un,
[property: CliOption("--up")] string Up,
[property: CliOption("--update-version")] string UpdateVersion
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}
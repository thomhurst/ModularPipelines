using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dls", "fs", "list")]
public record AzDlsFsListOptions(
[property: CliOption("--path")] string Path
) : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}
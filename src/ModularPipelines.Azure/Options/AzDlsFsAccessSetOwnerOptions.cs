using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dls", "fs", "access", "set-owner")]
public record AzDlsFsAccessSetOwnerOptions(
[property: CliOption("--path")] string Path
) : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliOption("--group")]
    public string? Group { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--owner")]
    public string? Owner { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}
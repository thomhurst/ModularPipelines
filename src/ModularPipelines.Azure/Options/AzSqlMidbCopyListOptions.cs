using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "midb", "copy", "list")]
public record AzSqlMidbCopyListOptions : AzOptions
{
    [CliOption("--dest-mi")]
    public string? DestMi { get; set; }

    [CliOption("--dest-resource-group")]
    public string? DestResourceGroup { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--latest")]
    public bool? Latest { get; set; }

    [CliOption("--managed-instance")]
    public string? ManagedInstance { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}
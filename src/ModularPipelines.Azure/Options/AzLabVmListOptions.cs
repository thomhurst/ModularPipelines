using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lab", "vm", "list")]
public record AzLabVmListOptions(
[property: CliOption("--lab-name")] string LabName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--all")]
    public string? All { get; set; }

    [CliOption("--claimable")]
    public string? Claimable { get; set; }

    [CliOption("--environment")]
    public string? Environment { get; set; }

    [CliOption("--expand")]
    public string? Expand { get; set; }

    [CliOption("--filters")]
    public string? Filters { get; set; }

    [CliOption("--object-id")]
    public string? ObjectId { get; set; }

    [CliOption("--order-by")]
    public string? OrderBy { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}
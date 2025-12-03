using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dt", "job", "import", "cancel")]
public record AzDtJobImportCancelOptions(
[property: CliOption("--dt-name")] string DtName,
[property: CliOption("--job-id")] string JobId
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}
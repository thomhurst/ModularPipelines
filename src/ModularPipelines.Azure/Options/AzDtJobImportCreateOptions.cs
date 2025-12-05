using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dt", "job", "import", "create")]
public record AzDtJobImportCreateOptions(
[property: CliOption("--data-file")] string DataFile,
[property: CliOption("--dt-name")] string DtName,
[property: CliOption("--ibc")] string Ibc,
[property: CliOption("--input-storage-account")] int InputStorageAccount
) : AzOptions
{
    [CliOption("--job-id")]
    public string? JobId { get; set; }

    [CliOption("--obc")]
    public string? Obc { get; set; }

    [CliOption("--of")]
    public string? Of { get; set; }

    [CliOption("--osa")]
    public string? Osa { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}
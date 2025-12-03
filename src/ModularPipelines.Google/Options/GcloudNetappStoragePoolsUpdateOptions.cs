using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netapp", "storage-pools", "update")]
public record GcloudNetappStoragePoolsUpdateOptions(
[property: CliArgument] string StoragePool,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--active-directory")]
    public string? ActiveDirectory { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--capacity")]
    public string? Capacity { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage-mover", "project", "list")]
public record AzStorageMoverProjectListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--storage-mover-name")] string StorageMoverName
) : AzOptions
{
    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }
}
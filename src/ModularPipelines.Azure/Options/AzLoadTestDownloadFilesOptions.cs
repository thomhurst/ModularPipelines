using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("load", "test", "download-files")]
public record AzLoadTestDownloadFilesOptions(
[property: CliOption("--load-test-resource")] string LoadTestResource,
[property: CliOption("--path")] string Path,
[property: CliOption("--test-id")] string TestId
) : AzOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}
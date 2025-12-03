using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("load", "test", "file", "upload")]
public record AzLoadTestFileUploadOptions(
[property: CliOption("--load-test-resource")] string LoadTestResource,
[property: CliOption("--path")] string Path,
[property: CliOption("--test-id")] string TestId
) : AzOptions
{
    [CliOption("--file-type")]
    public string? FileType { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}
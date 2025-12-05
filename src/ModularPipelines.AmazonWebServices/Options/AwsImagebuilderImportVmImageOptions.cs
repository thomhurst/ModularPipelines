using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("imagebuilder", "import-vm-image")]
public record AwsImagebuilderImportVmImageOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--semantic-version")] string SemanticVersion,
[property: CliOption("--platform")] string Platform,
[property: CliOption("--vm-import-task-id")] string VmImportTaskId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--os-version")]
    public string? OsVersion { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
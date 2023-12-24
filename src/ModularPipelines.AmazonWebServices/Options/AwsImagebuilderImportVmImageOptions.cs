using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("imagebuilder", "import-vm-image")]
public record AwsImagebuilderImportVmImageOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--semantic-version")] string SemanticVersion,
[property: CommandSwitch("--platform")] string Platform,
[property: CommandSwitch("--vm-import-task-id")] string VmImportTaskId
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--os-version")]
    public string? OsVersion { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
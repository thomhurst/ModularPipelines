using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("imagebuilder", "import-component")]
public record AwsImagebuilderImportComponentOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--semantic-version")] string SemanticVersion,
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--format")] string Format,
[property: CommandSwitch("--platform")] string Platform
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--change-description")]
    public string? ChangeDescription { get; set; }

    [CommandSwitch("--data")]
    public string? Data { get; set; }

    [CommandSwitch("--uri")]
    public string? Uri { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
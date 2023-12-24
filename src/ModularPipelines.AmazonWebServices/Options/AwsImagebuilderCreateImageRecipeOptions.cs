using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("imagebuilder", "create-image-recipe")]
public record AwsImagebuilderCreateImageRecipeOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--semantic-version")] string SemanticVersion,
[property: CommandSwitch("--components")] string[] Components,
[property: CommandSwitch("--parent-image")] string ParentImage
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--block-device-mappings")]
    public string[]? BlockDeviceMappings { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--working-directory")]
    public string? AwsImagWorkingDirectory { get; set; }

    [CommandSwitch("--additional-instance-configuration")]
    public string? AdditionalInstanceConfiguration { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
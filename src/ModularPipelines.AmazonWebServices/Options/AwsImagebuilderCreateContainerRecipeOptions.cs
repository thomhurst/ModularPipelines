using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("imagebuilder", "create-container-recipe")]
public record AwsImagebuilderCreateContainerRecipeOptions(
[property: CommandSwitch("--container-type")] string ContainerType,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--semantic-version")] string SemanticVersion,
[property: CommandSwitch("--components")] string[] Components,
[property: CommandSwitch("--parent-image")] string ParentImage,
[property: CommandSwitch("--target-repository")] string TargetRepository
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--instance-configuration")]
    public string? InstanceConfiguration { get; set; }

    [CommandSwitch("--dockerfile-template-data")]
    public string? DockerfileTemplateData { get; set; }

    [CommandSwitch("--dockerfile-template-uri")]
    public string? DockerfileTemplateUri { get; set; }

    [CommandSwitch("--platform-override")]
    public string? PlatformOverride { get; set; }

    [CommandSwitch("--image-os-version-override")]
    public string? ImageOsVersionOverride { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--working-directory")]
    public string? AwsImagWorkingDirectory { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
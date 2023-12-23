using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "import-workspace-image")]
public record AwsWorkspacesImportWorkspaceImageOptions(
[property: CommandSwitch("--ec2-image-id")] string Ec2ImageId,
[property: CommandSwitch("--ingestion-process")] string IngestionProcess,
[property: CommandSwitch("--image-name")] string ImageName,
[property: CommandSwitch("--image-description")] string ImageDescription
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--applications")]
    public string[]? Applications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "import-workspace-image")]
public record AwsWorkspacesImportWorkspaceImageOptions(
[property: CliOption("--ec2-image-id")] string Ec2ImageId,
[property: CliOption("--ingestion-process")] string IngestionProcess,
[property: CliOption("--image-name")] string ImageName,
[property: CliOption("--image-description")] string ImageDescription
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--applications")]
    public string[]? Applications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
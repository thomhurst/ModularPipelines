using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "create-folder")]
public record AwsQuicksightCreateFolderOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--folder-id")] string FolderId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--folder-type")]
    public string? FolderType { get; set; }

    [CliOption("--parent-folder-arn")]
    public string? ParentFolderArn { get; set; }

    [CliOption("--permissions")]
    public string[]? Permissions { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--sharing-model")]
    public string? SharingModel { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
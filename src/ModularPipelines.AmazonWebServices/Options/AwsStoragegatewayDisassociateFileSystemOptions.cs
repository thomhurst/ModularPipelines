using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "disassociate-file-system")]
public record AwsStoragegatewayDisassociateFileSystemOptions(
[property: CliOption("--file-system-association-arn")] string FileSystemAssociationArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
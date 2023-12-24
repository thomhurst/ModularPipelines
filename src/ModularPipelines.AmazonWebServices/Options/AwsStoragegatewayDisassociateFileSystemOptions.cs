using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "disassociate-file-system")]
public record AwsStoragegatewayDisassociateFileSystemOptions(
[property: CommandSwitch("--file-system-association-arn")] string FileSystemAssociationArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
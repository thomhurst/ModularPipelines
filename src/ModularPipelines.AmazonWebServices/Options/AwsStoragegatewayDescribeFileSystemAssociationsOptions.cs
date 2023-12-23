using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "describe-file-system-associations")]
public record AwsStoragegatewayDescribeFileSystemAssociationsOptions(
[property: CommandSwitch("--file-system-association-arn-list")] string[] FileSystemAssociationArnList
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
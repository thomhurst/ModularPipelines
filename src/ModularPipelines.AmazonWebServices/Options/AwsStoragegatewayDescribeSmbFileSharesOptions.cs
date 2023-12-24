using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "describe-smb-file-shares")]
public record AwsStoragegatewayDescribeSmbFileSharesOptions(
[property: CommandSwitch("--file-share-arn-list")] string[] FileShareArnList
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
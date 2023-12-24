using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "get-access-grant")]
public record AwsS3controlGetAccessGrantOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--access-grant-id")] string AccessGrantId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
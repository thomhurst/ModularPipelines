using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "create-access-point-for-object-lambda")]
public record AwsS3controlCreateAccessPointForObjectLambdaOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--configuration")] string Configuration
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
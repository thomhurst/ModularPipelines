using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm-sap", "put-resource-permission")]
public record AwsSsmSapPutResourcePermissionOptions(
[property: CommandSwitch("--action-type")] string ActionType,
[property: CommandSwitch("--source-resource-arn")] string SourceResourceArn,
[property: CommandSwitch("--resource-arn")] string ResourceArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
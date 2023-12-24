using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm-sap", "get-resource-permission")]
public record AwsSsmSapGetResourcePermissionOptions(
[property: CommandSwitch("--resource-arn")] string ResourceArn
) : AwsOptions
{
    [CommandSwitch("--action-type")]
    public string? ActionType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
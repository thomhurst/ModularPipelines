using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm-sap", "delete-resource-permission")]
public record AwsSsmSapDeleteResourcePermissionOptions(
[property: CommandSwitch("--resource-arn")] string ResourceArn
) : AwsOptions
{
    [CommandSwitch("--action-type")]
    public string? ActionType { get; set; }

    [CommandSwitch("--source-resource-arn")]
    public string? SourceResourceArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
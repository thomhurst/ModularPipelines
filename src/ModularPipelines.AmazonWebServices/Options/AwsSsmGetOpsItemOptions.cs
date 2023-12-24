using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "get-ops-item")]
public record AwsSsmGetOpsItemOptions(
[property: CommandSwitch("--ops-item-id")] string OpsItemId
) : AwsOptions
{
    [CommandSwitch("--ops-item-arn")]
    public string? OpsItemArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "put-inventory")]
public record AwsSsmPutInventoryOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--items")] string[] Items
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
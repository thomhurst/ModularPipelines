using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "update-association-status")]
public record AwsSsmUpdateAssociationStatusOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--association-status")] string AssociationStatus
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "register-volume")]
public record AwsOpsworksRegisterVolumeOptions(
[property: CommandSwitch("--stack-id")] string StackId
) : AwsOptions
{
    [CommandSwitch("--ec2-volume-id")]
    public string? Ec2VolumeId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
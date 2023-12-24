using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datasync", "create-location-fsx-ontap")]
public record AwsDatasyncCreateLocationFsxOntapOptions(
[property: CommandSwitch("--protocol")] string Protocol,
[property: CommandSwitch("--security-group-arns")] string[] SecurityGroupArns,
[property: CommandSwitch("--storage-virtual-machine-arn")] string StorageVirtualMachineArn
) : AwsOptions
{
    [CommandSwitch("--subdirectory")]
    public string? Subdirectory { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
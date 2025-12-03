using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datasync", "create-location-fsx-ontap")]
public record AwsDatasyncCreateLocationFsxOntapOptions(
[property: CliOption("--protocol")] string Protocol,
[property: CliOption("--security-group-arns")] string[] SecurityGroupArns,
[property: CliOption("--storage-virtual-machine-arn")] string StorageVirtualMachineArn
) : AwsOptions
{
    [CliOption("--subdirectory")]
    public string? Subdirectory { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
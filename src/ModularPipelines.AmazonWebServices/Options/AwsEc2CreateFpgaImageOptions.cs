using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-fpga-image")]
public record AwsEc2CreateFpgaImageOptions(
[property: CommandSwitch("--input-storage-location")] string InputStorageLocation
) : AwsOptions
{
    [CommandSwitch("--logs-storage-location")]
    public string? LogsStorageLocation { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
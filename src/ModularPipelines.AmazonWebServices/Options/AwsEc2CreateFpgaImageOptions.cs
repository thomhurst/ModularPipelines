using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-fpga-image")]
public record AwsEc2CreateFpgaImageOptions(
[property: CliOption("--input-storage-location")] string InputStorageLocation
) : AwsOptions
{
    [CliOption("--logs-storage-location")]
    public string? LogsStorageLocation { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "get-instance-types-from-instance-requirements")]
public record AwsEc2GetInstanceTypesFromInstanceRequirementsOptions(
[property: CliOption("--architecture-types")] string[] ArchitectureTypes,
[property: CliOption("--virtualization-types")] string[] VirtualizationTypes,
[property: CliOption("--instance-requirements")] string InstanceRequirements
) : AwsOptions
{
    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
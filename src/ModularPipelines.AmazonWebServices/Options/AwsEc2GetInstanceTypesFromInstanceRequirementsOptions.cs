using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "get-instance-types-from-instance-requirements")]
public record AwsEc2GetInstanceTypesFromInstanceRequirementsOptions(
[property: CommandSwitch("--architecture-types")] string[] ArchitectureTypes,
[property: CommandSwitch("--virtualization-types")] string[] VirtualizationTypes,
[property: CommandSwitch("--instance-requirements")] string InstanceRequirements
) : AwsOptions
{
    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eks", "describe-update")]
public record AwsEksDescribeUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--update-id")] string UpdateId
) : AwsOptions
{
    [CommandSwitch("--nodegroup-name")]
    public string? NodegroupName { get; set; }

    [CommandSwitch("--addon-name")]
    public string? AddonName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "update-group")]
public record AwsIamUpdateGroupOptions(
[property: CommandSwitch("--group-name")] string GroupName
) : AwsOptions
{
    [CommandSwitch("--new-path")]
    public string? NewPath { get; set; }

    [CommandSwitch("--new-group-name")]
    public string? NewGroupName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
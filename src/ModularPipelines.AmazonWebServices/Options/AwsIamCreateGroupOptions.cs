using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "create-group")]
public record AwsIamCreateGroupOptions(
[property: CommandSwitch("--group-name")] string GroupName
) : AwsOptions
{
    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
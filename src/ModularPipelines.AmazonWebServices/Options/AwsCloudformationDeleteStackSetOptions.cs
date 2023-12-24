using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "delete-stack-set")]
public record AwsCloudformationDeleteStackSetOptions(
[property: CommandSwitch("--stack-set-name")] string StackSetName
) : AwsOptions
{
    [CommandSwitch("--call-as")]
    public string? CallAs { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
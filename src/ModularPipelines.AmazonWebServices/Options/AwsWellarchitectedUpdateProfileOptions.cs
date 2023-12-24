using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "update-profile")]
public record AwsWellarchitectedUpdateProfileOptions(
[property: CommandSwitch("--profile-arn")] string ProfileArn
) : AwsOptions
{
    [CommandSwitch("--profile-description")]
    public string? ProfileDescription { get; set; }

    [CommandSwitch("--profile-questions")]
    public string[]? ProfileQuestions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
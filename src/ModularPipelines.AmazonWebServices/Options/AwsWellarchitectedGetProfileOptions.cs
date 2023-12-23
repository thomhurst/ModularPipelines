using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "get-profile")]
public record AwsWellarchitectedGetProfileOptions(
[property: CommandSwitch("--profile-arn")] string ProfileArn
) : AwsOptions
{
    [CommandSwitch("--profile-version")]
    public string? ProfileVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
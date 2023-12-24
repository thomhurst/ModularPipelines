using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "delete-account-setting")]
public record AwsEcsDeleteAccountSettingOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--principal-arn")]
    public string? PrincipalArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
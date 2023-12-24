using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "put-account-setting")]
public record AwsEcsPutAccountSettingOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--value")] string Value
) : AwsOptions
{
    [CommandSwitch("--principal-arn")]
    public string? PrincipalArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
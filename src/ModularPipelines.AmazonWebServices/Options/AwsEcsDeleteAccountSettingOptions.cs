using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "delete-account-setting")]
public record AwsEcsDeleteAccountSettingOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--principal-arn")]
    public string? PrincipalArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
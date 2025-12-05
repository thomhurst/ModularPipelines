using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "put-account-setting")]
public record AwsEcsPutAccountSettingOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--value")] string Value
) : AwsOptions
{
    [CliOption("--principal-arn")]
    public string? PrincipalArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
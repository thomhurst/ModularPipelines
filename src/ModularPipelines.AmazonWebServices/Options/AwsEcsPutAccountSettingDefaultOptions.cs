using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "put-account-setting-default")]
public record AwsEcsPutAccountSettingDefaultOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--value")] string Value
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
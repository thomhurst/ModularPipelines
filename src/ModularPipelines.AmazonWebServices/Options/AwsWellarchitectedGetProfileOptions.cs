using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "get-profile")]
public record AwsWellarchitectedGetProfileOptions(
[property: CliOption("--profile-arn")] string ProfileArn
) : AwsOptions
{
    [CliOption("--profile-version")]
    public string? ProfileVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
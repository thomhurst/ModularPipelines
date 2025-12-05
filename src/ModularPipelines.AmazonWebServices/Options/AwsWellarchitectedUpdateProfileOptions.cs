using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "update-profile")]
public record AwsWellarchitectedUpdateProfileOptions(
[property: CliOption("--profile-arn")] string ProfileArn
) : AwsOptions
{
    [CliOption("--profile-description")]
    public string? ProfileDescription { get; set; }

    [CliOption("--profile-questions")]
    public string[]? ProfileQuestions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
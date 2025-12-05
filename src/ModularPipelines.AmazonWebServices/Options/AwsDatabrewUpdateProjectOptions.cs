using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("databrew", "update-project")]
public record AwsDatabrewUpdateProjectOptions(
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--sample")]
    public string? Sample { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
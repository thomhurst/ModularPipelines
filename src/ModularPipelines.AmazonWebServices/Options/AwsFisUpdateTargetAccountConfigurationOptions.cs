using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fis", "update-target-account-configuration")]
public record AwsFisUpdateTargetAccountConfigurationOptions(
[property: CliOption("--experiment-template-id")] string ExperimentTemplateId,
[property: CliOption("--account-id")] string AccountId
) : AwsOptions
{
    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fis", "get-target-account-configuration")]
public record AwsFisGetTargetAccountConfigurationOptions(
[property: CliOption("--experiment-template-id")] string ExperimentTemplateId,
[property: CliOption("--account-id")] string AccountId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "wait", "type-registration-complete")]
public record AwsCloudformationWaitTypeRegistrationCompleteOptions(
[property: CliOption("--registration-token")] string RegistrationToken
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
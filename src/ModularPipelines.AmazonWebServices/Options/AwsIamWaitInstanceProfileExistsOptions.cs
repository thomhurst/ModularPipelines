using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "wait", "instance-profile-exists")]
public record AwsIamWaitInstanceProfileExistsOptions(
[property: CliOption("--instance-profile-name")] string InstanceProfileName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
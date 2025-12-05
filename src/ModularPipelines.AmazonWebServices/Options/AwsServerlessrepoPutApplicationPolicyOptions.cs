using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("serverlessrepo", "put-application-policy")]
public record AwsServerlessrepoPutApplicationPolicyOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--statements")] string[] Statements
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
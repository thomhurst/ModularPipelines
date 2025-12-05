using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("serverlessrepo", "get-application")]
public record AwsServerlessrepoGetApplicationOptions(
[property: CliOption("--application-id")] string ApplicationId
) : AwsOptions
{
    [CliOption("--semantic-version")]
    public string? SemanticVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
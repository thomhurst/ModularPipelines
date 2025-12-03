using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("m2", "get-application-version")]
public record AwsM2GetApplicationVersionOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--application-version")] int ApplicationVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
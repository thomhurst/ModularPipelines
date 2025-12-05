using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("finspace", "get-kx-user")]
public record AwsFinspaceGetKxUserOptions(
[property: CliOption("--user-name")] string UserName,
[property: CliOption("--environment-id")] string EnvironmentId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
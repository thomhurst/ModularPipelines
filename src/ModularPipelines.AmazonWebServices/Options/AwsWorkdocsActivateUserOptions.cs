using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workdocs", "activate-user")]
public record AwsWorkdocsActivateUserOptions(
[property: CliOption("--user-id")] string UserId
) : AwsOptions
{
    [CliOption("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
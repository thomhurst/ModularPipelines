using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("finspace", "update-kx-user")]
public record AwsFinspaceUpdateKxUserOptions(
[property: CliOption("--environment-id")] string EnvironmentId,
[property: CliOption("--user-name")] string UserName,
[property: CliOption("--iam-role")] string IamRole
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
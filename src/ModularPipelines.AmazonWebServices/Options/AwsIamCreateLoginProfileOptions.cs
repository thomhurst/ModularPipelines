using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "create-login-profile")]
public record AwsIamCreateLoginProfileOptions(
[property: CliOption("--user-name")] string UserName,
[property: CliOption("--password")] string Password
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "update-login-profile")]
public record AwsIamUpdateLoginProfileOptions(
[property: CliOption("--user-name")] string UserName
) : AwsOptions
{
    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
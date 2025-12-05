using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "create-user")]
public record AwsElasticacheCreateUserOptions(
[property: CliOption("--user-id")] string UserId,
[property: CliOption("--user-name")] string UserName,
[property: CliOption("--engine")] string Engine,
[property: CliOption("--access-string")] string AccessString
) : AwsOptions
{
    [CliOption("--passwords")]
    public string[]? Passwords { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--authentication-mode")]
    public string? AuthenticationMode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
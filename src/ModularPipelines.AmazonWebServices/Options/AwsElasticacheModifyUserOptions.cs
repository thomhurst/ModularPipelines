using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "modify-user")]
public record AwsElasticacheModifyUserOptions(
[property: CliOption("--user-id")] string UserId
) : AwsOptions
{
    [CliOption("--access-string")]
    public string? AccessString { get; set; }

    [CliOption("--append-access-string")]
    public string? AppendAccessString { get; set; }

    [CliOption("--passwords")]
    public string[]? Passwords { get; set; }

    [CliOption("--authentication-mode")]
    public string? AuthenticationMode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
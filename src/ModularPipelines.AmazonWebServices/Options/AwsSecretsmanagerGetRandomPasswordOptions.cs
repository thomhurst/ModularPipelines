using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secretsmanager", "get-random-password")]
public record AwsSecretsmanagerGetRandomPasswordOptions : AwsOptions
{
    [CliOption("--password-length")]
    public long? PasswordLength { get; set; }

    [CliOption("--exclude-characters")]
    public string? ExcludeCharacters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
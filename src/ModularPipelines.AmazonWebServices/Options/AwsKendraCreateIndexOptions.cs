using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "create-index")]
public record AwsKendraCreateIndexOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--edition")]
    public string? Edition { get; set; }

    [CliOption("--server-side-encryption-configuration")]
    public string? ServerSideEncryptionConfiguration { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--user-token-configurations")]
    public string[]? UserTokenConfigurations { get; set; }

    [CliOption("--user-context-policy")]
    public string? UserContextPolicy { get; set; }

    [CliOption("--user-group-resolution-configuration")]
    public string? UserGroupResolutionConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
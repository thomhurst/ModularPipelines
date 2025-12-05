using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("nimble", "create-studio")]
public record AwsNimbleCreateStudioOptions(
[property: CliOption("--admin-role-arn")] string AdminRoleArn,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--studio-name")] string StudioName,
[property: CliOption("--user-role-arn")] string UserRoleArn
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--studio-encryption-configuration")]
    public string? StudioEncryptionConfiguration { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
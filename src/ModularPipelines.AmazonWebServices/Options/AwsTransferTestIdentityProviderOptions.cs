using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "test-identity-provider")]
public record AwsTransferTestIdentityProviderOptions(
[property: CliOption("--server-id")] string ServerId,
[property: CliOption("--user-name")] string UserName
) : AwsOptions
{
    [CliOption("--server-protocol")]
    public string? ServerProtocol { get; set; }

    [CliOption("--source-ip")]
    public string? SourceIp { get; set; }

    [CliOption("--user-password")]
    public string? UserPassword { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
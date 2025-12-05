using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("signer", "start-signing-job")]
public record AwsSignerStartSigningJobOptions(
[property: CliOption("--source")] string Source,
[property: CliOption("--destination")] string Destination,
[property: CliOption("--profile-name")] string ProfileName
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--profile-owner")]
    public string? ProfileOwner { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "update-agreement")]
public record AwsTransferUpdateAgreementOptions(
[property: CliOption("--agreement-id")] string AgreementId,
[property: CliOption("--server-id")] string ServerId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--local-profile-id")]
    public string? LocalProfileId { get; set; }

    [CliOption("--partner-profile-id")]
    public string? PartnerProfileId { get; set; }

    [CliOption("--base-directory")]
    public string? BaseDirectory { get; set; }

    [CliOption("--access-role")]
    public string? AccessRole { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
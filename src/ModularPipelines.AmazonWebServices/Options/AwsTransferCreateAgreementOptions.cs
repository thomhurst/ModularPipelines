using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "create-agreement")]
public record AwsTransferCreateAgreementOptions(
[property: CliOption("--server-id")] string ServerId,
[property: CliOption("--local-profile-id")] string LocalProfileId,
[property: CliOption("--partner-profile-id")] string PartnerProfileId,
[property: CliOption("--base-directory")] string BaseDirectory,
[property: CliOption("--access-role")] string AccessRole
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
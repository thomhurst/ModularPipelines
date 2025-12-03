using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("signer", "list-signing-jobs")]
public record AwsSignerListSigningJobsOptions : AwsOptions
{
    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--platform-id")]
    public string? PlatformId { get; set; }

    [CliOption("--requested-by")]
    public string? RequestedBy { get; set; }

    [CliOption("--signature-expires-before")]
    public long? SignatureExpiresBefore { get; set; }

    [CliOption("--signature-expires-after")]
    public long? SignatureExpiresAfter { get; set; }

    [CliOption("--job-invoker")]
    public string? JobInvoker { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
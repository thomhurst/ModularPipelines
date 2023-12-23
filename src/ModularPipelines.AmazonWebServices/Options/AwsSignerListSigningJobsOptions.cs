using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("signer", "list-signing-jobs")]
public record AwsSignerListSigningJobsOptions : AwsOptions
{
    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--platform-id")]
    public string? PlatformId { get; set; }

    [CommandSwitch("--requested-by")]
    public string? RequestedBy { get; set; }

    [CommandSwitch("--signature-expires-before")]
    public long? SignatureExpiresBefore { get; set; }

    [CommandSwitch("--signature-expires-after")]
    public long? SignatureExpiresAfter { get; set; }

    [CommandSwitch("--job-invoker")]
    public string? JobInvoker { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
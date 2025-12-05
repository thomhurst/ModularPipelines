using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auditmanager", "register-account")]
public record AwsAuditmanagerRegisterAccountOptions : AwsOptions
{
    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--delegated-admin-account")]
    public string? DelegatedAdminAccount { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
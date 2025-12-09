using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector2", "disable-delegated-admin-account")]
public record AwsInspector2DisableDelegatedAdminAccountOptions(
[property: CliOption("--delegated-admin-account-id")] string DelegatedAdminAccountId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
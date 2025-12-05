using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "update-access-policy")]
public record AwsIotsitewiseUpdateAccessPolicyOptions(
[property: CliOption("--access-policy-id")] string AccessPolicyId,
[property: CliOption("--access-policy-identity")] string AccessPolicyIdentity,
[property: CliOption("--access-policy-resource")] string AccessPolicyResource,
[property: CliOption("--access-policy-permission")] string AccessPolicyPermission
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
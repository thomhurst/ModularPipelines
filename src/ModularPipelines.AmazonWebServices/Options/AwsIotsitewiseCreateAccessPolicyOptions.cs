using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "create-access-policy")]
public record AwsIotsitewiseCreateAccessPolicyOptions(
[property: CliOption("--access-policy-identity")] string AccessPolicyIdentity,
[property: CliOption("--access-policy-resource")] string AccessPolicyResource,
[property: CliOption("--access-policy-permission")] string AccessPolicyPermission
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
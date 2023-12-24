using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "create-access-policy")]
public record AwsIotsitewiseCreateAccessPolicyOptions(
[property: CommandSwitch("--access-policy-identity")] string AccessPolicyIdentity,
[property: CommandSwitch("--access-policy-resource")] string AccessPolicyResource,
[property: CommandSwitch("--access-policy-permission")] string AccessPolicyPermission
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
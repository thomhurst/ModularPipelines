using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "list-policies-granting-service-access")]
public record AwsIamListPoliciesGrantingServiceAccessOptions(
[property: CommandSwitch("--arn")] string Arn,
[property: CommandSwitch("--service-namespaces")] string[] ServiceNamespaces
) : AwsOptions
{
    [CommandSwitch("--marker")]
    public string? Marker { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
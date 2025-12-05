using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "list-policies-granting-service-access")]
public record AwsIamListPoliciesGrantingServiceAccessOptions(
[property: CliOption("--arn")] string Arn,
[property: CliOption("--service-namespaces")] string[] ServiceNamespaces
) : AwsOptions
{
    [CliOption("--marker")]
    public string? Marker { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
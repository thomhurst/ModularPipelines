using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "delete-access-entry")]
public record AwsEksDeleteAccessEntryOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--principal-arn")] string PrincipalArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "update-addon")]
public record AwsEksUpdateAddonOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--addon-name")] string AddonName
) : AwsOptions
{
    [CliOption("--addon-version")]
    public string? AddonVersion { get; set; }

    [CliOption("--service-account-role-arn")]
    public string? ServiceAccountRoleArn { get; set; }

    [CliOption("--resolve-conflicts")]
    public string? ResolveConflicts { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--configuration-values")]
    public string? ConfigurationValues { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
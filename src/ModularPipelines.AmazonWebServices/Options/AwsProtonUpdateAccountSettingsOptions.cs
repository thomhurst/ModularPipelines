using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "update-account-settings")]
public record AwsProtonUpdateAccountSettingsOptions : AwsOptions
{
    [CliOption("--pipeline-codebuild-role-arn")]
    public string? PipelineCodebuildRoleArn { get; set; }

    [CliOption("--pipeline-provisioning-repository")]
    public string? PipelineProvisioningRepository { get; set; }

    [CliOption("--pipeline-service-role-arn")]
    public string? PipelineServiceRoleArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("proton", "update-account-settings")]
public record AwsProtonUpdateAccountSettingsOptions : AwsOptions
{
    [CommandSwitch("--pipeline-codebuild-role-arn")]
    public string? PipelineCodebuildRoleArn { get; set; }

    [CommandSwitch("--pipeline-provisioning-repository")]
    public string? PipelineProvisioningRepository { get; set; }

    [CommandSwitch("--pipeline-service-role-arn")]
    public string? PipelineServiceRoleArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
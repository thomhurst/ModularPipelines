using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudcontrol", "list-resources")]
public record AwsCloudcontrolListResourcesOptions(
[property: CommandSwitch("--type-name")] string TypeName
) : AwsOptions
{
    [CommandSwitch("--type-version-id")]
    public string? TypeVersionId { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--resource-model")]
    public string? ResourceModel { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptunedata", "list-ml-endpoints")]
public record AwsNeptunedataListMlEndpointsOptions : AwsOptions
{
    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--neptune-iam-role-arn")]
    public string? NeptuneIamRoleArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
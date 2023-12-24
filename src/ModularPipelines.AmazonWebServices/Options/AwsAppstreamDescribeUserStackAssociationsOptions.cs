using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "describe-user-stack-associations")]
public record AwsAppstreamDescribeUserStackAssociationsOptions : AwsOptions
{
    [CommandSwitch("--stack-name")]
    public string? StackName { get; set; }

    [CommandSwitch("--user-name")]
    public string? UserName { get; set; }

    [CommandSwitch("--authentication-type")]
    public string? AuthenticationType { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
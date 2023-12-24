using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "create-default-roles")]
public record AwsEmrCreateDefaultRolesOptions : AwsOptions
{
    [CommandSwitch("--iam-endpoint")]
    public string? IamEndpoint { get; set; }
}
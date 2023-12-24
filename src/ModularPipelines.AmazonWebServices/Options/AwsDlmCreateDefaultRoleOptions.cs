using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dlm", "create-default-role")]
public record AwsDlmCreateDefaultRoleOptions : AwsOptions
{
    [CommandSwitch("--iam-endpoint")]
    public string? IamEndpoint { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }
}
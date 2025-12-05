using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dlm", "create-default-role")]
public record AwsDlmCreateDefaultRoleOptions : AwsOptions
{
    [CliOption("--iam-endpoint")]
    public string? IamEndpoint { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }
}
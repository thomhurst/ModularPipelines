using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "create-default-roles")]
public record AwsEmrCreateDefaultRolesOptions : AwsOptions
{
    [CliOption("--iam-endpoint")]
    public string? IamEndpoint { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm-sap", "get-application")]
public record AwsSsmSapGetApplicationOptions : AwsOptions
{
    [CommandSwitch("--application-id")]
    public string? ApplicationId { get; set; }

    [CommandSwitch("--application-arn")]
    public string? ApplicationArn { get; set; }

    [CommandSwitch("--app-registry-arn")]
    public string? AppRegistryArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
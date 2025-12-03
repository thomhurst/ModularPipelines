using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrass", "create-group-version")]
public record AwsGreengrassCreateGroupVersionOptions(
[property: CliOption("--group-id")] string GroupId
) : AwsOptions
{
    [CliOption("--amzn-client-token")]
    public string? AmznClientToken { get; set; }

    [CliOption("--connector-definition-version-arn")]
    public string? ConnectorDefinitionVersionArn { get; set; }

    [CliOption("--core-definition-version-arn")]
    public string? CoreDefinitionVersionArn { get; set; }

    [CliOption("--device-definition-version-arn")]
    public string? DeviceDefinitionVersionArn { get; set; }

    [CliOption("--function-definition-version-arn")]
    public string? FunctionDefinitionVersionArn { get; set; }

    [CliOption("--logger-definition-version-arn")]
    public string? LoggerDefinitionVersionArn { get; set; }

    [CliOption("--resource-definition-version-arn")]
    public string? ResourceDefinitionVersionArn { get; set; }

    [CliOption("--subscription-definition-version-arn")]
    public string? SubscriptionDefinitionVersionArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
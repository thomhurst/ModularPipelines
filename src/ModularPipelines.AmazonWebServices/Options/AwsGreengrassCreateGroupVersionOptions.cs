using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("greengrass", "create-group-version")]
public record AwsGreengrassCreateGroupVersionOptions(
[property: CommandSwitch("--group-id")] string GroupId
) : AwsOptions
{
    [CommandSwitch("--amzn-client-token")]
    public string? AmznClientToken { get; set; }

    [CommandSwitch("--connector-definition-version-arn")]
    public string? ConnectorDefinitionVersionArn { get; set; }

    [CommandSwitch("--core-definition-version-arn")]
    public string? CoreDefinitionVersionArn { get; set; }

    [CommandSwitch("--device-definition-version-arn")]
    public string? DeviceDefinitionVersionArn { get; set; }

    [CommandSwitch("--function-definition-version-arn")]
    public string? FunctionDefinitionVersionArn { get; set; }

    [CommandSwitch("--logger-definition-version-arn")]
    public string? LoggerDefinitionVersionArn { get; set; }

    [CommandSwitch("--resource-definition-version-arn")]
    public string? ResourceDefinitionVersionArn { get; set; }

    [CommandSwitch("--subscription-definition-version-arn")]
    public string? SubscriptionDefinitionVersionArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
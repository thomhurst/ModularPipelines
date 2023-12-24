using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "register-type")]
public record AwsCloudformationRegisterTypeOptions(
[property: CommandSwitch("--type-name")] string TypeName,
[property: CommandSwitch("--schema-handler-package")] string SchemaHandlerPackage
) : AwsOptions
{
    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--logging-config")]
    public string? LoggingConfig { get; set; }

    [CommandSwitch("--execution-role-arn")]
    public string? ExecutionRoleArn { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
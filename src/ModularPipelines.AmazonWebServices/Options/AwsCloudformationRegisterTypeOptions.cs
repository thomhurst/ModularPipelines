using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "register-type")]
public record AwsCloudformationRegisterTypeOptions(
[property: CliOption("--type-name")] string TypeName,
[property: CliOption("--schema-handler-package")] string SchemaHandlerPackage
) : AwsOptions
{
    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--logging-config")]
    public string? LoggingConfig { get; set; }

    [CliOption("--execution-role-arn")]
    public string? ExecutionRoleArn { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
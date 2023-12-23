using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "generate-db-auth-token")]
public record AwsRdsGenerateDbAuthTokenOptions(
[property: CommandSwitch("--hostname")] string Hostname,
[property: CommandSwitch("--port")] int Port,
[property: CommandSwitch("--username")] string Username
) : AwsOptions;
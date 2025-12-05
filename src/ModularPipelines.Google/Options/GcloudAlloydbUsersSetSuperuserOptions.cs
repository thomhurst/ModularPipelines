using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alloydb", "users", "set-superuser")]
public record GcloudAlloydbUsersSetSuperuserOptions(
[property: CliArgument] string Username,
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--region")] string Region,
[property: CliOption("--superuser")] string Superuser
) : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("sql")]
public class GcloudSqlSsl
{
    public GcloudSqlSsl(
        GcloudSqlSslClientCerts clientCerts
    )
    {
        ClientCerts = clientCerts;
    }

    public GcloudSqlSslClientCerts ClientCerts { get; }
}
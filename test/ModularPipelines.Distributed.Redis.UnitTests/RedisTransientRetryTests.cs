using System.Net.Sockets;
using Respire;

namespace ModularPipelines.Distributed.Redis.UnitTests;

public class RedisTransientRetryTests
{
    public static IEnumerable<Func<(Exception Exception, bool Transient)>> Classifications()
    {
        yield return () => (new RespireTimeoutException("GET", TimeSpan.FromSeconds(1)), true);
        yield return () => (new RespireConnectionException("socket closed"), true);
        yield return () => (new SocketException((int) SocketError.ConnectionRefused), true);
        yield return () => (new RespireAuthenticationException("WRONGPASS invalid username-password pair"), false);
        yield return () => (new RespireServerException("NOAUTH Authentication required.", "GET"), false);
        yield return () => (new ArgumentException("Invalid connection string."), false);
    }

    [Test]
    [MethodDataSource(nameof(Classifications))]
    public async Task Only_Transient_Failures_Are_Retried(Exception exception, bool transient) =>
        await Assert.That(RedisTransientRetry.IsTransient(exception)).IsEqualTo(transient);
}

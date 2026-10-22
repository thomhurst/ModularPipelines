namespace ModularPipelines.Distributed.Discovery.Redis;

internal static class RedisDiscoveryScripts
{
    internal const string Refresh = """
        if redis.call('GET', KEYS[1]) == ARGV[1] then
            return redis.call('PEXPIRE', KEYS[1], ARGV[2])
        end
        return 0
        """;

    internal const string Delete = """
        if redis.call('GET', KEYS[1]) == ARGV[1] then
            return redis.call('DEL', KEYS[1])
        end
        return 0
        """;
}

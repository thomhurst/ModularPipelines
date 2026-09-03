namespace ModularPipelines.Distributed.Discovery.Redis.UnitTests;

public class RedisDiscoveryNamespaceTests
{
    [Test]
    public async Task Consumer_Entry_Points_Are_In_The_Package_Root()
    {
        await Assert.That(typeof(RedisDiscoveryExtensions).Namespace)
            .IsEqualTo("ModularPipelines.Distributed.Discovery.Redis");
        await Assert.That(typeof(RedisDiscoveryOptions).Namespace)
            .IsEqualTo("ModularPipelines.Distributed.Discovery.Redis");
    }
}

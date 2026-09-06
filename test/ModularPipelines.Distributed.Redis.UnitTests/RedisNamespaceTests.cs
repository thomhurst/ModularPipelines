using ModularPipelines.Distributed.Redis;

namespace ModularPipelines.Distributed.Redis.UnitTests;

public class RedisNamespaceTests
{
    [Test]
    public async Task Consumer_Entry_Points_Are_In_The_Package_Root()
    {
        await Assert.That(typeof(RedisDistributedExtensions).Namespace)
            .IsEqualTo("ModularPipelines.Distributed.Redis");
        await Assert.That(typeof(RedisDistributedOptions).Namespace)
            .IsEqualTo("ModularPipelines.Distributed.Redis");
    }
}

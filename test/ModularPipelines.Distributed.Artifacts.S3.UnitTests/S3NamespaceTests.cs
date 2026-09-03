using ModularPipelines.Distributed.Artifacts.S3;

namespace ModularPipelines.Distributed.Artifacts.S3.UnitTests;

public class S3NamespaceTests
{
    [Test]
    public async Task Consumer_Entry_Points_Are_In_The_Package_Root()
    {
        await Assert.That(typeof(S3DistributedExtensions).Namespace)
            .IsEqualTo("ModularPipelines.Distributed.Artifacts.S3");
        await Assert.That(typeof(S3ArtifactOptions).Namespace)
            .IsEqualTo("ModularPipelines.Distributed.Artifacts.S3");
    }
}

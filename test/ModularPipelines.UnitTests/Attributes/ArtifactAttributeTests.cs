using ModularPipelines.Attributes;

namespace ModularPipelines.UnitTests.Attributes;

public class ArtifactAttributeTests
{
    [Test]
    [Arguments(null)]
    [Arguments("")]
    [Arguments(" ")]
    public async Task ProducesArtifactRejectsMissingName(string? name)
    {
        await Assert.That(() => new ProducesArtifactAttribute(name!, "output.txt"))
            .Throws<ArgumentException>();
    }

    [Test]
    [Arguments(null)]
    [Arguments("")]
    [Arguments(" ")]
    public async Task ProducesArtifactRejectsMissingPathPattern(string? pathPattern)
    {
        await Assert.That(() => new ProducesArtifactAttribute("output", pathPattern!))
            .Throws<ArgumentException>();
    }

    [Test]
    [Arguments(null)]
    [Arguments("")]
    [Arguments(" ")]
    public async Task ConsumesArtifactRejectsMissingName(string? name)
    {
        await Assert.That(() => new ConsumesArtifactAttribute(typeof(object), name!))
            .Throws<ArgumentException>();
    }

    [Test]
    public async Task ConsumesArtifactRejectsMissingProducerModule()
    {
        await Assert.That(() => new ConsumesArtifactAttribute(null!, "output"))
            .Throws<ArgumentNullException>();
    }
}

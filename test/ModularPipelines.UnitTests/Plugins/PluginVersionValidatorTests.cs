using ModularPipelines.Engine;
using ModularPipelines.Exceptions;

namespace ModularPipelines.UnitTests.Plugins;

public class PluginVersionValidatorTests
{
    [Test]
    public async Task Validate_WithoutAttribute_DoesNotThrow()
    {
        // Use an assembly without ModularPipelinesPluginAttribute
        var assembly = typeof(object).Assembly; // mscorlib has no plugin attribute

        await Assert.That(() => PluginVersionValidator.Validate(assembly, new Version(5, 0, 0)))
            .ThrowsNothing();
    }

    [Test]
    public async Task IsCompatible_WithoutAttribute_ReturnsTrue()
    {
        var assembly = typeof(object).Assembly;

        var result = PluginVersionValidator.IsCompatible(assembly, new Version(5, 0, 0));

        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task IsCompatible_WithNullVersion_ReturnsTrueForNoAttribute()
    {
        var assembly = typeof(object).Assembly;

        var result = PluginVersionValidator.IsCompatible(assembly, null);

        await Assert.That(result).IsTrue();
    }
}

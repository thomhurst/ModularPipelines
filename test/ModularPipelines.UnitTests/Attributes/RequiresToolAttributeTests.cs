using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Attributes;

public class RequiresToolAttributeTests
{
    [Test]
    public async Task Does_Not_Skip_Tool_On_Path()
    {
        var attribute = new RequiresToolAttribute("dotnet");

        var shouldSkip = await attribute.ShouldSkip(null!);

        await Assert.That(shouldSkip).IsFalse();
    }

    [Test]
    public async Task Skips_Tool_Not_On_Path()
    {
        var attribute = new RequiresToolAttribute($"missing-tool-{Guid.NewGuid():N}");

        var shouldSkip = await attribute.ShouldSkip(null!);

        await Assert.That(shouldSkip).IsTrue();
    }

    [Test]
    [Arguments(null)]
    [Arguments("")]
    [Arguments(" ")]
    public async Task Rejects_Missing_Tool_Name(string? tool)
    {
        await Assert.That(() => new RequiresToolAttribute(tool!))
            .Throws<ArgumentException>();
    }
}

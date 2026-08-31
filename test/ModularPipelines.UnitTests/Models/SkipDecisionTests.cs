using System.Runtime.CompilerServices;
using System.Text.Json;
using ModularPipelines.Models;

namespace ModularPipelines.UnitTests.Models;

public class SkipDecisionTests
{
    [Test]
    public async Task ImplicitConversions_AreNotExposed()
    {
        var conversions = typeof(SkipDecision)
            .GetMethods()
            .Where(method => method.Name == "op_Implicit")
            .Select(method =>
                $"{method.GetParameters().Single().ParameterType.FullName}->{method.ReturnType.FullName}")
            .ToArray();

        await Assert.That(conversions).IsEmpty();
    }

    [Test]
    public async Task ShouldSkip_IsInitOnly()
    {
        var setter = typeof(SkipDecision)
            .GetProperty(nameof(SkipDecision.ShouldSkip))!
            .SetMethod!;

        await Assert.That(setter.ReturnParameter.GetRequiredCustomModifiers())
            .Contains(typeof(IsExternalInit));
    }

    [Test]
    public async Task JsonRoundTrip_PreservesDecision()
    {
        var expected = SkipDecision.Skip("Serialized reason");

        var json = JsonSerializer.Serialize(expected);
        var actual = JsonSerializer.Deserialize<SkipDecision>(json);

        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task Skip()
    {
        var skipDecision = SkipDecision.Skip("Blah!");

        using (Assert.Multiple())
        {
            await Assert.That(skipDecision.ShouldSkip).IsTrue();
            await Assert.That(skipDecision.Reason).IsEqualTo("Blah!");
        }
    }

    [Test]
    public async Task DoNotSkip()
    {
        var skipDecision = SkipDecision.DoNotSkip;

        using (Assert.Multiple())
        {
            await Assert.That(skipDecision.ShouldSkip).IsFalse();
            await Assert.That(skipDecision.Reason).IsNull();
        }
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task When(bool shouldSkip)
    {
        var skipDecision = SkipDecision.When(shouldSkip, "Blah!");

        using (Assert.Multiple())
        {
            await Assert.That(skipDecision.ShouldSkip).IsEqualTo(shouldSkip);
            await Assert.That(skipDecision.Reason).IsEqualTo(shouldSkip ? "Blah!" : null);
        }
    }
}

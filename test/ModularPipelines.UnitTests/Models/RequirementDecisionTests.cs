using ModularPipelines.Requirements;

namespace ModularPipelines.UnitTests.Models;

public class RequirementDecisionTests
{
    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task Json_RoundTrip_Preserves_Decision_And_Property_Names(bool passed)
    {
        var decision = passed ? RequirementDecision.Passed : RequirementDecision.Failed("missing tool");
        var json = System.Text.Json.JsonSerializer.Serialize(decision);
        var restored = System.Text.Json.JsonSerializer.Deserialize<RequirementDecision>(json)!;
        using var document = System.Text.Json.JsonDocument.Parse(json);

        using (Assert.Multiple())
        {
            await Assert.That(document.RootElement.GetProperty("IsSatisfied").GetBoolean()).IsEqualTo(passed);
            await Assert.That(document.RootElement.GetProperty("Reason").GetString()).IsEqualTo(decision.Reason);
            await Assert.That(restored.IsSatisfied).IsEqualTo(passed);
            await Assert.That(restored.Reason).IsEqualTo(decision.Reason);
        }
    }

    [Test]
    public async Task True_Implicit_Cast()
    {
        RequirementDecision requirementDecision = true;

        using (Assert.Multiple())
        {
            await Assert.That(requirementDecision.IsSatisfied).IsTrue();
            await Assert.That(requirementDecision.Reason).IsNull();
        }
    }

    [Test]
    public async Task False_Implicit_Cast()
    {
        RequirementDecision requirementDecision = false;

        using (Assert.Multiple())
        {
            await Assert.That(requirementDecision.IsSatisfied).IsFalse();
            await Assert.That(requirementDecision.Reason).IsNull();
        }
    }

    [Test]
    public async Task Only_Bool_Implicit_Conversion_Remains()
    {
        var implicitConversions = typeof(RequirementDecision)
            .GetMethods()
            .Where(static method => method.Name == "op_Implicit")
            .ToArray();

        await Assert.That(implicitConversions).HasSingleItem();
        await Assert.That(implicitConversions[0].GetParameters()).HasSingleItem();
        await Assert.That(implicitConversions[0].GetParameters()[0].ParameterType).IsEqualTo(typeof(bool));
        await Assert.That(implicitConversions[0].ReturnType).IsEqualTo(typeof(RequirementDecision));
    }

    [Test]
    public async Task Failed()
    {
        var requirementDecision = RequirementDecision.Failed("Blah!");

        using (Assert.Multiple())
        {
            await Assert.That(requirementDecision.IsSatisfied).IsFalse();
            await Assert.That(requirementDecision.Reason).IsEqualTo("Blah!");
        }
    }

    [Test]
    public async Task Passed()
    {
        var requirementDecision = RequirementDecision.Passed;

        using (Assert.Multiple())
        {
            await Assert.That(requirementDecision.IsSatisfied).IsTrue();
            await Assert.That(requirementDecision.Reason).IsNull();
        }
    }

    [Test]
    public async Task Surface_Uses_IsSatisfied_And_Factories()
    {
        var type = typeof(RequirementDecision);

        using (Assert.Multiple())
        {
            await Assert.That(type.GetProperty("IsSatisfied")).IsNotNull();
            await Assert.That(type.GetProperty("Success")).IsNull();
            await Assert.That(type.GetMethod("Of")).IsNull();
            await Assert.That(type.GetField("Passed")).IsNotNull();
            await Assert.That(type.GetMethod("Failed")).IsNotNull();
        }
    }
}

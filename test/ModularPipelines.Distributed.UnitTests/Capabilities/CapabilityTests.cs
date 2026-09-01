using System.Text.Json;
using ModularPipelines.Attributes;

namespace ModularPipelines.Distributed.UnitTests.Capabilities;

public class CapabilityTests
{
    [Test]
    public async Task Equality_Is_Case_Insensitive()
    {
        Capability upper = "Docker";
        Capability lower = "docker";

        using (Assert.Multiple())
        {
            await Assert.That(upper).IsEqualTo(lower);
            await Assert.That(upper.GetHashCode()).IsEqualTo(lower.GetHashCode());
            await Assert.That(new HashSet<Capability> { upper }.Contains(lower)).IsTrue();
        }
    }

    [Test]
    public async Task Known_Names_Work_For_Attributes()
    {
        var attribute = new RequiresCapabilityAttribute(
            Capability.Names.Linux,
            Capability.Names.Docker);

        await Assert.That(attribute.Capabilities)
            .IsEquivalentTo([Capability.Linux.Name, Capability.Docker.Name]);
    }

    [Test]
    public async Task Attribute_Copies_Capability_Names()
    {
        var names = new[] { Capability.Names.Docker };
        var attribute = new RequiresCapabilityAttribute(names);

        names[0] = Capability.Names.Gpu;

        await Assert.That(attribute.Capabilities).IsEquivalentTo([Capability.Names.Docker]);
    }

    [Test]
    public async Task Json_Wire_Format_Is_A_Plain_String()
    {
        var json = JsonSerializer.Serialize(Capability.Docker);
        var roundTripped = JsonSerializer.Deserialize<Capability>(json);

        using (Assert.Multiple())
        {
            await Assert.That(json).IsEqualTo("\"docker\"");
            await Assert.That(roundTripped).IsEqualTo(Capability.Docker);
        }
    }

    [Test]
    public async Task Implicit_String_Conversions_Preserve_Custom_Names()
    {
        Capability capability = "high-memory";
        string name = capability;

        using (Assert.Multiple())
        {
            await Assert.That(capability.Name).IsEqualTo("high-memory");
            await Assert.That(name).IsEqualTo("high-memory");
        }
    }

    [Test]
    public async Task Empty_Name_Is_Rejected()
    {
        await Assert.That(() => new Capability(" "))
            .Throws<ArgumentException>();
    }
}

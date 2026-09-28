using System.Text.Json;
using ModularPipelines.Attributes;

namespace ModularPipelines.Distributed.UnitTests.Capabilities;

public class CapabilityRequirementTests
{
    [Test]
    public async Task None_Is_Satisfied_By_Any_Worker()
    {
        using (Assert.Multiple())
        {
            await Assert.That(CapabilityRequirement.None.IsSatisfiedBy([])).IsTrue();
            await Assert.That(CapabilityRequirement.None.IsSatisfiedBy([Capability.Linux])).IsTrue();
        }
    }

    [Test]
    public async Task AllOf_Requires_Every_Capability()
    {
        var requirement = CapabilityRequirement.AllOf(Capability.Linux, Capability.Docker);

        using (Assert.Multiple())
        {
            await Assert.That(requirement.IsSatisfiedBy([Capability.Linux, Capability.Docker])).IsTrue();
            await Assert.That(requirement.IsSatisfiedBy([Capability.Linux])).IsFalse();
        }
    }

    [Test]
    public async Task AnyOf_Requires_One_Capability()
    {
        var requirement = CapabilityRequirement.AnyOf(Capability.Linux, Capability.MacOS);

        using (Assert.Multiple())
        {
            await Assert.That(requirement.IsSatisfiedBy([Capability.Linux])).IsTrue();
            await Assert.That(requirement.IsSatisfiedBy([Capability.MacOS])).IsTrue();
            await Assert.That(requirement.IsSatisfiedBy([Capability.Windows])).IsFalse();
        }
    }

    [Test]
    public async Task Matching_Is_Case_Insensitive()
    {
        var requirement = CapabilityRequirement.AllOf(new Capability("Docker"));

        await Assert.That(requirement.IsSatisfiedBy([new Capability("docker")])).IsTrue();
    }

    [Test]
    public async Task Or_Distributes_Over_Clauses()
    {
        // (linux & docker) | windows == (linux | windows) & (docker | windows)
        var requirement = CapabilityRequirement.AllOf(Capability.Linux, Capability.Docker)
            .Or(CapabilityRequirement.AllOf(Capability.Windows));

        using (Assert.Multiple())
        {
            await Assert.That(requirement).IsEqualTo(CapabilityRequirement.Create(
            [
                [Capability.Linux, Capability.Windows],
                [Capability.Docker, Capability.Windows],
            ]));
            await Assert.That(requirement.IsSatisfiedBy([Capability.Windows])).IsTrue();
            await Assert.That(requirement.IsSatisfiedBy([Capability.Linux, Capability.Docker])).IsTrue();
            await Assert.That(requirement.IsSatisfiedBy([Capability.Linux])).IsFalse();
        }
    }

    [Test]
    public async Task Or_With_None_Is_None()
    {
        var requirement = CapabilityRequirement.AllOf(Capability.Linux).Or(CapabilityRequirement.None);

        await Assert.That(requirement.IsEmpty).IsTrue();
    }

    [Test]
    public async Task Clauses_Are_Normalized()
    {
        var requirement = CapabilityRequirement.Create(
        [
            [Capability.MacOS, Capability.Linux, Capability.Linux],
            [Capability.Linux],
            [new Capability("LINUX")],
            [Capability.Docker],
        ]);

        using (Assert.Multiple())
        {
            // The (linux | macos) clause adds nothing once linux is required.
            await Assert.That(requirement.Clauses.Count).IsEqualTo(2);
            await Assert.That(requirement).IsEqualTo(CapabilityRequirement.AllOf(Capability.Docker, Capability.Linux));
            await Assert.That(requirement.GetHashCode())
                .IsEqualTo(CapabilityRequirement.AllOf(Capability.Linux, Capability.Docker).GetHashCode());
        }
    }

    [Test]
    public async Task Clauses_Are_Compared_By_Capability_Not_Display_Text()
    {
        // Both clauses display as "a | b", but one needs the custom "a | b" capability.
        var requirement = CapabilityRequirement.Create([[new Capability("a | b")], [new Capability("a"), new Capability("b")]]);

        using (Assert.Multiple())
        {
            await Assert.That(requirement.Clauses.Count).IsEqualTo(2);
            await Assert.That(requirement.IsSatisfiedBy([new Capability("a")])).IsFalse();
            await Assert.That(requirement.IsSatisfiedBy([new Capability("a | b"), new Capability("a")])).IsTrue();
        }
    }

    [Test]
    public async Task Empty_Clause_Is_Rejected()
    {
        await Assert.That(() => CapabilityRequirement.AnyOf())
            .Throws<ArgumentException>();
    }

    [Test]
    public async Task Different_Operating_Systems_Are_Not_Satisfiable()
    {
        using (Assert.Multiple())
        {
            await Assert.That(CapabilityRequirement.AllOf(Capability.Linux, Capability.Windows).IsSatisfiable)
                .IsFalse();
            await Assert.That(CapabilityRequirement.AnyOf(Capability.Linux, Capability.MacOS)
                    .And(CapabilityRequirement.AnyOf(Capability.MacOS, Capability.Windows))
                    .IsSatisfiable)
                .IsTrue();
            await Assert.That(CapabilityRequirement.AnyOf(Capability.Linux, Capability.Docker)
                    .And(CapabilityRequirement.AllOf(Capability.Windows))
                    .IsSatisfiable)
                .IsTrue();
            await Assert.That(CapabilityRequirement.AllOf(Capability.Docker, Capability.Gpu).IsSatisfiable)
                .IsTrue();
        }
    }

    [Test]
    public async Task ToString_Describes_Clauses()
    {
        var requirement = CapabilityRequirement.AnyOf(Capability.Linux, Capability.MacOS)
            .And(CapabilityRequirement.AllOf(Capability.Docker));

        using (Assert.Multiple())
        {
            await Assert.That(requirement.ToString()).IsEqualTo("docker & (linux | macos)");
            await Assert.That(CapabilityRequirement.None.ToString()).IsEqualTo("none");
        }
    }

    [Test]
    public async Task Json_Wire_Format_Is_An_Array_Of_Clauses()
    {
        var requirement = CapabilityRequirement.AnyOf(Capability.Linux, Capability.MacOS)
            .And(CapabilityRequirement.AllOf(Capability.Docker));

        var json = JsonSerializer.Serialize(requirement);
        var roundTripped = JsonSerializer.Deserialize<CapabilityRequirement>(json);

        using (Assert.Multiple())
        {
            await Assert.That(json).IsEqualTo("""[["docker"],["linux","macos"]]""");
            await Assert.That(roundTripped).IsEqualTo(requirement);
            await Assert.That(JsonSerializer.Serialize(CapabilityRequirement.None)).IsEqualTo("[]");
        }
    }

    [Test]
    public async Task Json_Rejects_Empty_Clause()
    {
        await Assert.That(() => JsonSerializer.Deserialize<CapabilityRequirement>("[[]]"))
            .Throws<JsonException>();
    }

    [Test]
    public async Task Any_Attribute_Copies_Capability_Names()
    {
        var names = new[] { Capability.Names.Linux, Capability.Names.MacOS };
        var attribute = new RequiresAnyCapabilityAttribute(names);

        names[0] = Capability.Names.Gpu;

        await Assert.That(attribute.Capabilities)
            .IsEquivalentTo([Capability.Names.Linux, Capability.Names.MacOS]);
    }

    [Test]
    public async Task Operating_System_Provider_Advertises_Current_Platform()
    {
        var capabilities = (await new OperatingSystemCapabilityProvider()
                .GetCapabilitiesAsync(CancellationToken.None))
            .ToArray();

        await Assert.That(capabilities).IsEquivalentTo([Capability.CurrentOperatingSystem!.Value]);
    }

    [Test]
    public async Task Local_Capabilities_Union_Options_And_Providers()
    {
        var capabilities = await LocalCapabilities.ResolveAsync(
            new DistributedOptions { Capabilities = [Capability.Gpu] },
            [new OperatingSystemCapabilityProvider(), new FixedCapabilityProvider(Capability.Docker)],
            CancellationToken.None);

        await Assert.That(capabilities).IsEquivalentTo(
            [Capability.Gpu, Capability.Docker, Capability.CurrentOperatingSystem!.Value]);
    }

    [Test]
    public async Task Registry_Probes_Providers_Once_With_The_Application_Stopping_Token()
    {
        using var stopping = new CancellationTokenSource();
        var lifetime = new Moq.Mock<Microsoft.Extensions.Hosting.IHostApplicationLifetime>();
        lifetime.Setup(x => x.ApplicationStopping).Returns(stopping.Token);
        var provider = new RecordingCapabilityProvider();
        var registry = new LocalCapabilityRegistry(
            Microsoft.Extensions.Options.Options.Create(new DistributedOptions()),
            [provider],
            lifetime.Object);

        await registry.GetAsync(CancellationToken.None);
        await registry.GetAsync(CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(provider.Calls).IsEqualTo(1);
            await Assert.That(provider.Token).IsEqualTo(stopping.Token);
        }
    }

    private sealed class RecordingCapabilityProvider : ICapabilityProvider
    {
        public int Calls { get; private set; }

        public CancellationToken Token { get; private set; }

        public Task<IEnumerable<Capability>> GetCapabilitiesAsync(CancellationToken cancellationToken)
        {
            Calls++;
            Token = cancellationToken;
            return Task.FromResult<IEnumerable<Capability>>([Capability.Gpu]);
        }
    }

    private sealed class FixedCapabilityProvider(params Capability[] capabilities) : ICapabilityProvider
    {
        public Task<IEnumerable<Capability>> GetCapabilitiesAsync(CancellationToken cancellationToken) =>
            Task.FromResult<IEnumerable<Capability>>(capabilities);
    }
}

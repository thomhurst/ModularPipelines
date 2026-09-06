using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class CobraCliScraperTests
{
    [Test]
    public async Task Repeatable_Noun_Phrases_Produce_Collection_Options()
    {
        const string helpText = """
            Initialize a service

            Usage: fake service init [OPTIONS]

            Options:
              --external-ca external-ca   Specifications of one or more certificate signing endpoints
            """;
        var command = await new TestCobraCliScraper().Parse(
            ["fake", "service", "init"],
            helpText);

        var option = command!.Options.Single();
        using (Assert.Multiple())
        {
            await Assert.That(option.AcceptsMultipleValues).IsTrue();
            await Assert.That(option.CSharpType).IsEqualTo("IEnumerable<string>?");
        }
    }

    [Test]
    public async Task Quoted_Kubectl_Style_Defaults_Containing_Colons_Stay_Out_Of_Descriptions()
    {
        // minikube (kubectl-style rows) prints the default inside quotes; the value itself may
        // contain colons, which must not be mistaken for the description separator.
        const string helpText = """
            Starts a local Kubernetes cluster

            Usage: fake start [OPTIONS]

            Options:
                  --base-image='gcr.io/k8s-minikube/kicbase:v0.0.51@sha256:4a1c825b61479e6c898851ea66f13c620aaeab6002746e95067fc2c4b38a0b24': The base image to use for docker/podman drivers. Intended for local development.
                  --iso-url='[https://storage.googleapis.com/minikube/iso/minikube-v1.39.0-amd64.iso,https://github.com/kubernetes/minikube/releases/download/v1.39.0/minikube-v1.39.0-amd64.iso]': Locations to fetch the minikube ISO from.
                  --kvm-qemu-uri='qemu:///system': The KVM QEMU connection URI. (kvm2 driver only)
                  --memory='': Amount of RAM to allocate to Kubernetes (format: <number>[<unit>], where unit = b, k, m or g).
                  --nodes=1: The total number of nodes to spin up.
            """;
        var command = await new TestCobraCliScraper().Parse(["fake", "start"], helpText);

        string Description(string switchName) =>
            command!.Options.Single(option => option.SwitchName == switchName).Description!;

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Select(option => option.SwitchName))
                .IsEquivalentTo(["--base-image", "--iso-url", "--kvm-qemu-uri", "--memory", "--nodes"]);
            await Assert.That(Description("--base-image"))
                .IsEqualTo("The base image to use for docker/podman drivers. Intended for local development.");
            await Assert.That(Description("--iso-url"))
                .IsEqualTo("Locations to fetch the minikube ISO from.");
            await Assert.That(Description("--kvm-qemu-uri"))
                .IsEqualTo("The KVM QEMU connection URI. (kvm2 driver only)");
            await Assert.That(Description("--memory"))
                .IsEqualTo("Amount of RAM to allocate to Kubernetes (format: <number>[<unit>], where unit = b, k, m or g).");
            await Assert.That(command.Options.Single(option => option.SwitchName == "--kvm-qemu-uri").CSharpType)
                .IsEqualTo("string?");
            await Assert.That(command.Options.Single(option => option.SwitchName == "--nodes").CSharpType)
                .IsEqualTo("int?");
        }
    }

    [Test]
    public async Task Short_Command_Descriptions_Are_Preserved()
    {
        const string helpText = """
            Usage: fake report usage

            Disk usage

            Options:
              --verbose   Show more details
            """;
        var command = await new TestCobraCliScraper().Parse(
            ["fake", "report", "usage"],
            helpText);

        await Assert.That(command!.Description).IsEqualTo("Disk usage");
    }

    [Test]
    public async Task Usage_Continuations_Are_Not_Command_Descriptions()
    {
        const string helpText = """
            Usage:
              fake report usage

            Disk usage

            Options:
              --verbose   Show more details
            """;
        var command = await new TestCobraCliScraper().Parse(
            ["fake", "report", "usage"],
            helpText);

        await Assert.That(command!.Description).IsEqualTo("Disk usage");
    }

    private sealed class TestCobraCliScraper : CobraCliScraper
    {
        public TestCobraCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<TestCobraCliScraper>.Instance)
        {
        }

        public override string ToolName => "fake";

        public override string NamespacePrefix => "Fake";

        public override string TargetNamespace => "ModularPipelines.Fake";

        public override string OutputDirectory => "src/ModularPipelines.Fake";

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }
    }
}

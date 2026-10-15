using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.Generated;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    private static readonly string[] KubectlGetResourceNames = ["pods", "web"];

    [Test]
    public async Task Kubectl_Secret_Choices_Dispatch_To_Children_Without_Parent_Operands()
    {
        var commands = await ScrapeSynopsisCommands();
        var parent = commands.Single(command => command.FullCommand == "kubectl create secret");
        await Assert.That(parent.PositionalArguments).IsEmpty();
        await Assert.That(parent.RequiredAlternativeGroups).IsEmpty();
        var parentType = await CompileSetOptions(parent);
        await Assert.That(parentType.GetConstructors().Single().GetParameters()).IsEmpty();

        foreach (var variant in new[] { "docker-registry", "generic", "tls" })
        {
            var child = commands.Single(command => command.FullCommand == $"kubectl create secret {variant}");
            var childType = await CompileSetOptions(child);
            var instance = Activator.CreateInstance(childType, ["my-secret"])!;
            await Assert.That(RenderResourceOperands(instance)).IsEquivalentTo(["my-secret"]);
        }
    }

    [Test]
    public async Task Kubectl_Get_Output_Choices_Do_Not_Add_Operands_Or_Consume_Type()
    {
        var command = (await ScrapeSynopsisCommands()).Single(command => command.FullCommand == "kubectl get");
        await Assert.That(command.PositionalArguments.Select(argument => argument.PropertyName))
            .IsEquivalentTo(["TypeVersionGroupName"]);
        await Assert.That(command.Options.Select(option => option.SwitchName)).Contains("--output");
        var options = await CompileSetOptions(command);
        var instance = Activator.CreateInstance(options)!;
        options.GetProperty("TypeVersionGroupName")!.SetValue(instance, KubectlGetResourceNames);
        options.GetProperty("Output")!.SetValue(instance, "json");
        await Assert.That(RenderResourceOperands(instance)).IsEquivalentTo(["pods", "web"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
        var output = options.GetProperty("Output")!;
        var attribute = output.GetCustomAttributesData().Single(attribute => attribute.AttributeType.Name == nameof(CliOptionAttribute));
        var rendered = new CommandArgumentBuilder().BuildArguments(
            [new OptionPart(output.Name, output.GetValue, new CliOptionAttribute((string) attribute.ConstructorArguments.Single().Value!))], instance);
        await Assert.That(rendered).IsEquivalentTo(["--output", "json"], TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    private static async Task<List<CliCommandDefinition>> ScrapeSynopsisCommands()
    {
        var scraper = new KubectlCliScraper(new SynopsisExecutor(),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger<KubectlCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }
        return commands;
    }

    private sealed class SynopsisExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            string? workingDirectory = null, CancellationToken cancellationToken = default) =>
            Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardError = string.Empty,
                StandardOutput = arguments switch
                {
                    "--help" => "Available Commands:\n  create  Create resources\n  get  Display resources\n",
                    "create --help" => "Available Commands:\n  secret  Create a secret\n",
                    "create secret --help" => """
                        Create a secret.

                        Available Commands:
                          docker-registry  Create a registry secret
                          generic          Create a generic secret
                          tls              Create a TLS secret

                        Usage:
                          kubectl create secret (docker-registry | generic | tls)
                        """,
                    "create secret docker-registry --help" => "Usage: kubectl create secret docker-registry NAME [options]",
                    "create secret generic --help" => "Usage: kubectl create secret generic NAME [options]",
                    "create secret tls --help" => "Usage: kubectl create secret tls NAME [options]",
                    "get --help" => """
                        Usage:
                          kubectl get [(-o|--output=)json|yaml|wide|name] (TYPE[.VERSION][.GROUP] [NAME | -l label] | TYPE[.VERSION][.GROUP]/NAME ...) [options]

                        Options:
                          -o, --output string    Output format.
                          -l, --selector string  Label selector.
                        """,
                    "options" => """
                        The following options can be passed to any command:
                          --namespace string    Namespace scope.
                          --context string      Context to use.
                          --kubeconfig string   Path to kubeconfig.
                        """,
                    "version --client" => "Client Version: v1.37.1",
                    _ => throw new InvalidOperationException($"Unexpected command: {arguments}"),
                },
            });

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }
}

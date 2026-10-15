using System.ComponentModel.DataAnnotations;
using System.Reflection;
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
    public async Task Kubectl_Delete_Preserves_Multiple_Names_And_Selection_Alternatives()
    {
        var command = (await ScrapeSynopsisCommands()).Single(command => command.FullCommand == "kubectl delete");
        var name = command.PositionalArguments.Single(argument => argument.PropertyName == "Name");
        await Assert.That(name.IsVariadic).IsTrue();
        var options = await CompileSetOptions(command);
        var instance = Activator.CreateInstance(options)!;
        options.GetProperty("Type")!.SetValue(instance, "pod,service");
        options.GetProperty("Name")!.SetValue(instance, new[] { "baz", "foo" });
        await Assert.That(RenderResourceOperands(instance)).IsEquivalentTo(["pod,service", "baz", "foo"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
        await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();

        (string? Type, string? Property, object? Value, bool Valid)[] selections =
        [
            (null, null, null, false),
            ("pods", null, null, false),
            ("pods/web", null, null, true),
            ("pods", "All", true, true),
            ("pods", "All", false, false),
            ("pods", "Selector", "app=web", true),
            ("pods", "FieldSelector", "metadata.name=web", true),
            (null, "Filename", new[] { "pod.json" }, true),
            (null, "Kustomize", "overlay", true),
        ];
        foreach (var selection in selections)
        {
            instance = Activator.CreateInstance(options)!;
            options.GetProperty("Type")!.SetValue(instance, selection.Type);
            if (selection.Property is not null)
            {
                options.GetProperty(selection.Property)!.SetValue(instance, selection.Value);
            }
            await Assert.That(!((IValidatableObject) instance).Validate(new(instance)).Any())
                .IsEqualTo(selection.Valid).Because(selection.ToString());
        }
    }

    [Test]
    public async Task Kubectl_Deployment_Requires_Images_And_Preserves_Repeated_Options()
    {
        var command = (await ScrapeSynopsisCommands()).Single(command => command.FullCommand == "kubectl create deployment");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--image").IsRequired).IsTrue();
        var options = await CompileSetOptions(command);
        var constructor = options.GetConstructors().Single();
        await Assert.That(constructor.GetParameters().Select(parameter => parameter.Name)).Contains("Image");
        foreach (var images in new string[]?[] { null, [] })
        {
            var values = constructor.GetParameters().Select(parameter => parameter.Name == "Image" ? (object?) images : "my-dep").ToArray();
            var error = await Assert.That(() => constructor.Invoke(values)).Throws<TargetInvocationException>();
            await Assert.That(error!.InnerException).IsAssignableTo<ArgumentException>();
        }
        var suppliedImages = new[] { "busybox:latest", "nginx" };
        var arguments = constructor.GetParameters().Select(parameter => parameter.Name == "Image" ? (object) suppliedImages : "my-dep").ToArray();
        var instance = constructor.Invoke(arguments);
        var property = options.GetProperty("Image")!;
        var rendered = new CommandArgumentBuilder().BuildArguments(
            [new OptionPart(property.Name, property.GetValue, new CliOptionAttribute("--image") { Format = OptionFormat.EqualsSeparated })], instance);
        await Assert.That(rendered).IsEquivalentTo(["--image=busybox:latest", "--image=nginx"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

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
                    "--help" => "Available Commands:\n  create  Create resources\n  get  Display resources\n  delete  Delete resources\n",
                    "create --help" => "Available Commands:\n  secret  Create a secret\n  deployment  Create a deployment\n",
                    "delete --help" => """
                        Usage:
                          kubectl delete ([-f FILENAME] | [-k DIRECTORY] | TYPE [(NAME | -l label | --all)])

                        Options:
                          -f, --filename strings     Files identifying resources.
                          -k, --kustomize string     Kustomization directory.
                              --all=false: Select all resources.
                          -l, --selector string      Label selector.
                              --field-selector string  Field selector.
                        """,
                    "create deployment --help" => """
                        Usage:
                          kubectl create deployment NAME --image=image -- [COMMAND] [args...]

                        Options:
                          --image strings     Image names to run.
                          --replicas int32    Number of replicas.
                        """,
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

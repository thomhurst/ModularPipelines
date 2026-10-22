using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.UnitTests.Options;

public class CommandRecordPrintingTests
{
    private const string Password = "distinctive-password-6650";
    private const string StandardInput = "distinctive-stdin-6650";
    private const string EnvironmentSecret = "distinctive-environment-6650";

    [Test]
    [Arguments(null)]
    [Arguments("")]
    [Arguments(Password)]
    public async Task Credentials_Printing_Redacts_Password_Without_Changing_Record(string? password)
    {
        var credentials = new CommandLineCredentials
        {
            Domain = "example-domain",
            UserName = "example-user",
            Password = password,
            LoadUserProfile = true,
        };
        var copy = credentials with { };

        var printed = credentials.ToString();

        await Assert.That(printed).DoesNotContain(Password);
        await Assert.That(printed).Contains($"Password = {(password is null ? "<none>" : "<redacted>")}");
        await Assert.That(printed).Contains("UserName = example-user");
        await Assert.That(credentials.Password).IsEqualTo(password);
        await Assert.That(copy).IsEqualTo(credentials);
        await Assert.That(copy.GetHashCode()).IsEqualTo(credentials.GetHashCode());
        await Assert.That((credentials with { Password = "different" }) == credentials).IsFalse();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ExecutionOptions_Printing_Redacts_Secrets_Regardless_Of_Masking_Opt_In(bool standardInputIsSecret)
    {
        var environment = new PrintingEnvironment();
        var options = new CommandExecutionOptions
        {
            StandardInput = StandardInput,
            StandardInputIsSecret = standardInputIsSecret,
            CommandLineCredentials = new CommandLineCredentials { Password = Password },
            EnvironmentVariables = environment,
            WorkingDirectory = "example-directory",
        };

        var printed = options.ToString();

        await AssertNoSecrets(printed);
        await Assert.That(printed).Contains("StandardInput = <redacted>");
        await Assert.That(printed).Contains("EnvironmentVariables = <redacted>");
        await Assert.That(printed).Contains("WorkingDirectory = example-directory");
        await Assert.That(environment.ToStringCalled).IsFalse();
        await Assert.That(options.StandardInput).IsEqualTo(StandardInput);
        await Assert.That(options.CommandLineCredentials!.Password).IsEqualTo(Password);
        await Assert.That(options.EnvironmentVariables!["TOKEN"]).IsEqualTo(EnvironmentSecret);
        await Assert.That(options with { }).IsEqualTo(options);
        await Assert.That((options with { StandardInput = "different" }) == options).IsFalse();
        await Assert.That((options with { EnvironmentVariables = new Dictionary<string, string?>() }) == options).IsFalse();
    }

    [Test]
    public async Task ExecutionOptions_Printing_Handles_Null_Secrets()
    {
        var printed = new CommandExecutionOptions().ToString();

        await Assert.That(printed).Contains("StandardInput = <none>");
        await Assert.That(printed).Contains("EnvironmentVariables = <none>");
        await AssertNoSecrets(printed);
    }

    [Test]
    public async Task Derived_ExecutionOptions_Use_Redacted_Base_Printing()
    {
        var options = new DerivedExecutionOptions
        {
            StandardInput = StandardInput,
            CommandLineCredentials = new CommandLineCredentials { Password = Password },
            EnvironmentVariables = new PrintingEnvironment(),
        };

        var printed = options.ToString();

        await AssertNoSecrets(printed);
        await Assert.That(printed).Contains("Description = derived-options");
    }

    [Test]
    public async Task Invocation_Printing_Uses_Masked_View_And_Redacts_Nested_Secrets()
    {
        var environment = new PrintingEnvironment();
        var invocation = new CommandInvocation(
            new CommandLine("tool", [Password]),
            new PrintingToolOptions(),
            new CommandExecutionOptions
            {
                StandardInput = StandardInput,
                CommandLineCredentials = new CommandLineCredentials { Password = Password },
                EnvironmentVariables = environment,
            },
            "tool [masked]",
            "example-directory",
            environment);

        var printed = invocation.ToString();

        await AssertNoSecrets(printed);
        await Assert.That(printed).Contains("CommandInput = tool [masked]");
        await Assert.That(environment.ToStringCalled).IsFalse();
        await Assert.That(invocation.ExecutionOptions.StandardInput).IsEqualTo(StandardInput);
        await Assert.That(invocation.EnvironmentVariables["TOKEN"]).IsEqualTo(EnvironmentSecret);
        await Assert.That(invocation with { }).IsEqualTo(invocation);
        await Assert.That((invocation with { WorkingDirectory = "different" }) == invocation).IsFalse();
        var (commandLine, toolOptions, executionOptions, commandInput, workingDirectory, variables) = invocation;
        await Assert.That(commandLine).IsSameReferenceAs(invocation.CommandLine);
        await Assert.That(toolOptions).IsSameReferenceAs(invocation.ToolOptions);
        await Assert.That(executionOptions).IsSameReferenceAs(invocation.ExecutionOptions);
        await Assert.That(commandInput).IsEqualTo(invocation.CommandInput);
        await Assert.That(workingDirectory).IsEqualTo(invocation.WorkingDirectory);
        await Assert.That(variables).IsSameReferenceAs(environment);
    }

    [Test]
    public async Task Invocation_Printing_Handles_Null_Optional_Values()
    {
        var invocation = new CommandInvocation(new CommandLine("tool", []), null, new CommandExecutionOptions(), "tool", "directory", new Dictionary<string, string?>());

        var printed = invocation.ToString();

        await AssertNoSecrets(printed);
        await Assert.That(printed).Contains("ToolOptions = <none>");
        await Assert.That(printed).Contains("StandardInput = <none>");
    }

    private static async Task AssertNoSecrets(string printed)
    {
        await Assert.That(printed).DoesNotContain(Password);
        await Assert.That(printed).DoesNotContain(StandardInput);
        await Assert.That(printed).DoesNotContain(EnvironmentSecret);
    }

    private sealed class PrintingEnvironment : Dictionary<string, string?>
    {
        public PrintingEnvironment()
        {
            Add("TOKEN", EnvironmentSecret);
        }

        public bool ToStringCalled { get; private set; }

        public override string ToString()
        {
            ToStringCalled = true;
            return EnvironmentSecret;
        }
    }

    private sealed record PrintingToolOptions : CommandLineToolOptions
    {
        public override string ToString() => Password;
    }

    private sealed record DerivedExecutionOptions : CommandExecutionOptions
    {
        public string Description { get; init; } = "derived-options";
    }
}

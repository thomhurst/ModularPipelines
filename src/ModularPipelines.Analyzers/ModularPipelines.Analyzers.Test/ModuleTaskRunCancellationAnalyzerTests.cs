using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleTaskRunCancellationAnalyzerTests
{
    [TestMethod]
    [DataRow("token", true)]
    [DataRow("", false)]
    [DataRow("CancellationToken.None", false)]
    [DataRow("unrelatedToken", false)]
    public async Task Checks_Captured_Helper_Token_In_TaskRun_Lambda(string argument, bool forwardsToken)
    {
        var call = $"CreateTemplateXml({argument})";
        if (!forwardsToken)
        {
            call = $"{{|#0:{call}|}}";
        }

        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                CreateTemplateZip(CancellationToken.None, cancellationToken);

            private static Task<string> CreateTemplateZip(CancellationToken unrelatedToken, CancellationToken token) =>
                Task.Run(async () =>
                {
                    return await {{call}};
                }, token);

            private static Task<string> CreateTemplateXml(CancellationToken token = default) => Task.FromResult("xml");
            """);

        await VerifyCallAsync(source, forwardsToken, "CreateTemplateXml");
    }

    [TestMethod]
    [DataRow("cancellationToken", true)]
    [DataRow("", false)]
    [DataRow("CancellationToken.None", false)]
    [DataRow("new CancellationToken()", false)]
    public async Task Checks_Module_Token_In_TaskRun_Lambda(string argument, bool forwardsToken)
    {
        var call = string.IsNullOrEmpty(argument) ? "Task.Delay(1000)" : $"Task.Delay(1000, {argument})";
        if (!forwardsToken)
        {
            call = $"{{|#0:{call}|}}";
        }

        var source = ModuleSource($$"""
            protected override async Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            {
                await Task.Run(async () =>
                {
                    await {{call}};
                }, cancellationToken);
            }
            """);

        await VerifyCallAsync(source, forwardsToken, "Delay");
    }

    [TestMethod]
    [DataRow("async () => await Task.Delay(1, token)")]
    [DataRow("() => Task.Delay(1, token)")]
    [DataRow("async delegate { await Task.Delay(1, token); }")]
    [DataRow("Work")]
    public async Task Recognizes_Helper_Token_Captured_By_Nested_Callables(string callback)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static async Task HelperAsync(CancellationToken token)
            {
                await Task.Run({{callback}}, token);

                Task Work() => Task.Delay(1, token);
            }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    [DataRow("execute", false)]
    [DataRow("execute", true)]
    [DataRow("lifecycle", false)]
    [DataRow("lifecycle", true)]
    [DataRow("configuration", false)]
    [DataRow("configuration", true)]
    public async Task Checks_Helper_Token_In_Nested_Lambda_For_Each_Cancellation_Root(string root, bool forwardsToken)
    {
        var entry = root switch
        {
            "execute" => "protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => HelperAsync(cancellationToken);",
            "lifecycle" => "protected override Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => HelperAsync(cancellationToken);",
            _ => "protected override void Configure(ModuleConfigurationBuilder module) => module.WithSkipWhen(async (context, cancellationToken) => { await HelperAsync(cancellationToken); return false; });",
        };
        var call = forwardsToken ? "Task.Delay(1, token)" : "{|#0:Task.Delay(1)|}";
        var source = ModuleSource($$"""
            {{(root == "execute" ? "" : "protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => Task.CompletedTask;")}}
            {{entry}}

            private static Task HelperAsync(CancellationToken token)
            {
                return RunAsync();

                Task RunAsync() => Task.Run(async () =>
                {
                    await {{call}};
                }, token);
            }
            """);

        await VerifyCallAsync(source, forwardsToken, "Delay");
    }

    [TestMethod]
    public async Task Reports_Helper_Token_Not_Flowed_On_Every_Call_Path()
    {
        var source = ModuleSource("""
            protected override async Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            {
                await HelperAsync(cancellationToken, CancellationToken.None);
                await HelperAsync(CancellationToken.None, cancellationToken);
            }

            private static Task HelperAsync(CancellationToken firstToken, CancellationToken secondToken) =>
                {|#1:Task.Run(async () =>
                {
                    await {|#0:Task.Delay(1, firstToken)|};
                }, firstToken)|};
            """);

        // Neither helper parameter is guaranteed to carry the module token.
        // Task.Run and its awaited callback must both be diagnosed.
        await VerifyCS.VerifyAnalyzerAsync(
            source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(1).WithArguments("Run"));
    }

    [TestMethod]
    [DataRow("token = CancellationToken.None;", false)]
    [DataRow("token = default;", false)]
    [DataRow("Reset(ref token);", false)]
    [DataRow("ResetOut(out token);", false)]
    [DataRow("(token, _) = (CancellationToken.None, 0);", false)]
    [DataRow("token = CancellationToken.None;", true)]
    public async Task Reports_Overwritten_Helper_Token_Captured_By_Callback(string write, bool insideCallback)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                {{(insideCallback ? "" : write)}}
                return Task.Run(async () =>
                {
                    {{(insideCallback ? write : "")}}
                    await {|#0:Task.Delay(1, token)|};
                }, token);
            }

            private static void Reset(ref CancellationToken token) => token = default;
            private static void ResetOut(out CancellationToken token) => token = default;
            """);

        await VerifyCallAsync(source, false, "Delay");
    }

    [TestMethod]
    [DataRow("Read(in token);")]
    [DataRow("if (false) { token = default; }")]
    [DataRow("var dictionary = new System.Collections.Generic.Dictionary<CancellationToken, int>(); dictionary[token] = 1;")]
    [DataRow("var dictionary = new System.Collections.Generic.Dictionary<CancellationToken, int>(); (dictionary[token], _) = (1, 2);")]
    public async Task Recognizes_Unmodified_Helper_Token_Captured_By_Callback(string statement)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                {{statement}}
                return Task.Run(async () => await Task.Delay(1, token), token);
            }

            private static void Read(in CancellationToken token) { }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Helper_Token_Changed_After_Callback_Is_Scheduled()
    {
        var source = ModuleSource("""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                var work = Task.Run(async () => await {|#0:Task.Delay(1, token)|}, token);
                token = default;
                return work;
            }
            """);

        await VerifyCallAsync(source, false, "Delay");
    }

    private static string ModuleSource(string members) => $$"""
        {{TestSourceConstants.StandardModuleHeader}}

        public sealed class DummyModule : Module
        {
            {{members}}
        }
        """;

    private static Task VerifyCallAsync(string source, bool forwardsToken, string methodName) =>
        forwardsToken
            ? VerifyCS.VerifyAnalyzerAsync(source)
            : VerifyCS.VerifyAnalyzerAsync(
                source,
                VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                    .WithLocation(0)
                    .WithArguments(methodName));
}

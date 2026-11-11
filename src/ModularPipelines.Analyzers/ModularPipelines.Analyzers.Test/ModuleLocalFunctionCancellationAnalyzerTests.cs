using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleLocalFunctionCancellationAnalyzerTests
{
    [TestMethod]
    [DataRow("execute", "CancellationToken.None")]
    [DataRow("execute", "unrelatedToken")]
    [DataRow("lifecycle", "CancellationToken.None")]
    [DataRow("lifecycle", "unrelatedToken")]
    [DataRow("configuration", "CancellationToken.None")]
    [DataRow("configuration", "unrelatedToken")]
    public async Task Reports_Unrelated_Token_In_Returned_Lambda(string root, string argument)
    {
        var source = ModuleSource(root, $$"""
            private static Task HelperAsync(CancellationToken token)
            {
                var unrelatedToken = new CancellationToken();
                return Task.Run(() => {|#0:Task.Delay(1, {{argument}})|}, token);
            }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow("execute")]
    [DataRow("lifecycle")]
    [DataRow("configuration")]
    public async Task Recognizes_Token_Captured_By_Returned_Local_Function(string root)
    {
        var source = ModuleSource(root, """
            private static Task HelperAsync(CancellationToken token)
            {
                return RunAsync();

                Task RunAsync() => Task.Run(() => { }, token);
            }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    [DataRow("execute")]
    [DataRow("lifecycle")]
    [DataRow("configuration")]
    public async Task Recognizes_Token_Captured_By_Returned_Lambda(string root)
    {
        var source = ModuleSource(root, """
            private static Task HelperAsync(CancellationToken token)
            {
                return Task.Run(() => Task.Delay(1, token), token);
            }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    [DataRow("execute")]
    [DataRow("lifecycle")]
    [DataRow("configuration")]
    public async Task Recognizes_Token_Forwarded_To_Returned_Local_Function_In_Helper(string root)
    {
        var source = ModuleSource(root, """
            private static Task HelperAsync(CancellationToken token)
            {
                return RunAsync(token);

                Task RunAsync(CancellationToken localToken) => Task.Run(() => { }, localToken);
            }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    [DataRow("execute")]
    [DataRow("lifecycle")]
    [DataRow("configuration")]
    public async Task Recognizes_Token_Forwarded_Through_Returned_Local_Functions(string root)
    {
        var source = ModuleSource(root, """
            private static Task HelperAsync(CancellationToken token)
            {
                return Outer(token);

                Task Outer(CancellationToken outerToken) => Inner(outerToken);
                Task Inner(CancellationToken innerToken) => Task.Run(() => { }, innerToken);
            }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    [DataRow("execute", "CancellationToken.None")]
    [DataRow("execute", "unrelatedToken")]
    [DataRow("lifecycle", "CancellationToken.None")]
    [DataRow("lifecycle", "unrelatedToken")]
    [DataRow("configuration", "CancellationToken.None")]
    [DataRow("configuration", "unrelatedToken")]
    public async Task Reports_Unrelated_Token_In_Local_Function_In_Helper(string root, string argument)
    {
        var source = ModuleSource(root, $$"""
            private static Task HelperAsync(CancellationToken token)
            {
                return RunAsync(token);

                async Task RunAsync(CancellationToken localToken)
                {
                    var unrelatedToken = new CancellationToken();
                    await {|#0:Task.Run(() => { }, {{argument}})|};
                }
            }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Run"));
    }

    [TestMethod]
    [DataRow("execute", "CancellationToken.None")]
    [DataRow("execute", "unrelatedToken")]
    [DataRow("lifecycle", "CancellationToken.None")]
    [DataRow("lifecycle", "unrelatedToken")]
    [DataRow("configuration", "CancellationToken.None")]
    [DataRow("configuration", "unrelatedToken")]
    public async Task Reports_Unrelated_Token_Forwarded_To_Local_Function(string root, string argument)
    {
        var source = ModuleSource(root, $$"""
            private static Task HelperAsync(CancellationToken token)
            {
                var unrelatedToken = new CancellationToken();
                return {|#0:RunAsync({{argument}})|};

                async Task RunAsync(CancellationToken localToken)
                {
                    await {|#1:Task.Run(() => { }, localToken)|};
                }
            }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("RunAsync"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(1).WithArguments("Run"));
    }

    [TestMethod]
    [DataRow("execute", "CancellationToken.None")]
    [DataRow("execute", "unrelatedToken")]
    [DataRow("lifecycle", "CancellationToken.None")]
    [DataRow("lifecycle", "unrelatedToken")]
    [DataRow("configuration", "CancellationToken.None")]
    [DataRow("configuration", "unrelatedToken")]
    public async Task Reports_Unrelated_Token_Between_Local_Functions(string root, string argument)
    {
        var source = ModuleSource(root, $$"""
            private static Task HelperAsync(CancellationToken token)
            {
                return Outer(token);

                async Task Outer(CancellationToken outerToken)
                {
                    var unrelatedToken = new CancellationToken();
                    await {|#0:Inner({{argument}})|};
                }

                async Task Inner(CancellationToken innerToken)
                {
                    await {|#1:Task.Run(() => { }, innerToken)|};
                }
            }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Inner"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(1).WithArguments("Run"));
    }

    [TestMethod]
    [DataRow("execute")]
    [DataRow("lifecycle")]
    [DataRow("configuration")]
    public async Task Recognizes_Token_Forwarded_Through_Local_Wrapper_In_Entry(string root)
    {
        var source = ModuleSource(root, """
            private static Task HelperAsync(CancellationToken token)
            {
                return RunAsync(token);

                Task RunAsync(CancellationToken localToken) => Task.Run(() => { }, localToken);
            }
            """);
        var original = source;
        source = source.Replace("=> HelperAsync(cancellationToken);", "{ return Wrapper(); Task Wrapper() => HelperAsync(cancellationToken); }")
            .Replace("await HelperAsync(cancellationToken); return false;", "await Wrapper(); return false; Task Wrapper() => HelperAsync(cancellationToken);");

        Assert.AreNotEqual(original, source);
        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    [DataRow("execute", false)]
    [DataRow("lifecycle", false)]
    [DataRow("configuration", false)]
    [DataRow("execute", true)]
    [DataRow("lifecycle", true)]
    [DataRow("configuration", true)]
    public async Task Reports_Once_Per_Returned_Local_Function_Invocation(string root, bool chained)
    {
        var source = ModuleSource(root, $$"""
            private static Task HelperAsync(CancellationToken token)
            {
                return {{(chained ? "ForwardAsync(token)" : "RunAsync(token)")}};
                {{(chained ? "Task ForwardAsync(CancellationToken token) => RunAsync(token);" : "")}}
                Task RunAsync(CancellationToken localToken) => Task.WhenAll(
                    {|#0:Task.Run(() => { }, CancellationToken.None)|},
                    {|#1:Task.Delay(1, CancellationToken.None)|},
                    Task.Delay(1, localToken));
            }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Run"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(1).WithArguments("Delay"));
    }

    private static string ModuleSource(string root, string helper)
    {
        var entry = root switch
        {
            "execute" => "protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => HelperAsync(cancellationToken);",
            "lifecycle" => "protected override Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => HelperAsync(cancellationToken);",
            _ => "protected override void Configure(ModuleConfigurationBuilder module) => module.WithSkipWhen(async (context, cancellationToken) => { await HelperAsync(cancellationToken); return false; });",
        };

        return $$"""
            {{TestSourceConstants.StandardModuleHeader}}

            public sealed class BuildModule : Module
            {
                {{(root == "execute" ? "" : "protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => Task.CompletedTask;")}}
                {{entry}}

                {{helper}}
            }
            """;
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleCancellationTokenFrameworkPayloadAnalyzerTests
{
    [TestMethod]
    [DataRow("Task.FromResult(cancellationToken)", false)]
    [DataRow("Task.FromResult(CancellationToken.None)", true)]
    [DataRow("Task.FromResult(cancellationToken).ConfigureAwait(false)", false)]
    [DataRow("Task.FromResult(CancellationToken.None).ConfigureAwait(false)", true)]
    [DataRow("ValueTask.FromResult(cancellationToken)", false)]
    [DataRow("ValueTask.FromResult(CancellationToken.None)", true)]
    [DataRow("ValueTask.FromResult(cancellationToken).ConfigureAwait(false)", false)]
    [DataRow("ValueTask.FromResult(CancellationToken.None).ConfigureAwait(false)", true)]
    [DataRow("new ValueTask<CancellationToken>(cancellationToken)", false)]
    [DataRow("new ValueTask<CancellationToken>(CancellationToken.None)", true)]
    [DataRow("new ValueTask<CancellationToken>(Task.FromResult(cancellationToken))", false)]
    [DataRow("new ValueTask<CancellationToken>(Task.FromResult(CancellationToken.None))", true)]
    [DataRow("new ValueTask<CancellationToken>(cancellationToken).AsTask()", false)]
    [DataRow("new ValueTask<CancellationToken>(CancellationToken.None).AsTask()", true)]
    [DataRow("new ValueTask<CancellationToken>(Task.FromResult(cancellationToken)).ConfigureAwait(false)", false)]
    [DataRow("new ValueTask<CancellationToken>(Task.FromResult(CancellationToken.None)).ConfigureAwait(false)", true)]
    public async Task Tracks_Framework_Payloads(string expression, bool replaced)
    {
        await Verify($"var saved = {expression}; cancellationToken = default; cancellationToken = await saved;", replaced);
    }

    [TestMethod]
    [DataRow("Task.FromResult(cancellationToken)", false)]
    [DataRow("Task.FromResult(CancellationToken.None)", true)]
    [DataRow("ValueTask.FromResult(cancellationToken)", false)]
    [DataRow("ValueTask.FromResult(CancellationToken.None)", true)]
    [DataRow("new ValueTask<CancellationToken>(cancellationToken)", false)]
    [DataRow("new ValueTask<CancellationToken>(CancellationToken.None)", true)]
    public async Task Completed_Result_Tracks_Payload_Separately_From_Completion(string expression, bool replaced)
    {
        await Verify($"var saved = {expression}; await saved; cancellationToken = default; cancellationToken = saved.Result;", replaced);
    }

    [TestMethod]
    [DataRow("Task", false)]
    [DataRow("ValueTask", false)]
    [DataRow("Task", true)]
    [DataRow("ValueTask", true)]
    public async Task Direct_Await_Tracks_Original_And_Replaced_Payloads(string taskType, bool replaced)
    {
        var payload = replaced ? "CancellationToken.None" : "cancellationToken";
        await Verify("", replaced, token: $"await {taskType}.FromResult({payload}).ConfigureAwait(false)");
    }

    [TestMethod]
    [DataRow("Task", false)]
    [DataRow("ValueTask", false)]
    [DataRow("Task", true)]
    [DataRow("ValueTask", true)]
    public async Task Wrapper_Captures_Parameter_Value_At_Creation(string taskType, bool restore)
    {
        var restoration = restore ? "cancellationToken = original;" : "";
        await Verify($"var original = cancellationToken; cancellationToken = default; {restoration} cancellationToken = await {taskType}.FromResult(cancellationToken);", !restore);
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Custom_Awaiter_Does_Not_Prove_Payload(bool assign)
    {
        await Verify(assign ? "cancellationToken = await cancellationToken;" : "", true, """
            public static class Helpers
            {
                public static System.Runtime.CompilerServices.TaskAwaiter<CancellationToken> GetAwaiter(this CancellationToken token)
                    => Task.FromResult(CancellationToken.None).GetAwaiter();
            }
            """, assign ? "cancellationToken" : "await cancellationToken");
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Unrelated_Result_Does_Not_Prove_Payload(bool assign)
    {
        await Verify(assign ? "cancellationToken = new Container(cancellationToken).Result;" : "", true, """
            public class Container
            {
                public Container(CancellationToken token) { }
                public CancellationToken Result => CancellationToken.None;
            }
            """, assign ? "cancellationToken" : "new Container(cancellationToken).Result");
    }

    [TestMethod]
    public async Task Framework_Named_User_Extension_Does_Not_Prove_Payload()
    {
        await Verify("", true, """
            public static class Helpers
            {
                public static CancellationToken ConfigureAwait(this CancellationToken token, bool value) => default;
            }
            """, "cancellationToken.ConfigureAwait(false)");
    }

    [TestMethod]
    [DataRow("Task", false)]
    [DataRow("ValueTask", false)]
    [DataRow("Task", true)]
    [DataRow("ValueTask", true)]
    public async Task User_Conversions_Do_Not_Preserve_Payload(string taskType, bool wrapper)
    {
        var expression = wrapper
            ? $"({taskType}<CancellationToken>)(ReplacingAwaitable){taskType}.FromResult(cancellationToken)"
            : $"{taskType}.FromResult((CancellationToken)(ReplacingToken)cancellationToken)";
        var extra = $$"""
            public struct ReplacingToken
            {
                public static explicit operator ReplacingToken(CancellationToken value) => new();
                public static explicit operator CancellationToken(ReplacingToken value) => default;
            }
            public struct ReplacingAwaitable
            {
                public static explicit operator ReplacingAwaitable({{taskType}}<CancellationToken> value) => new();
                public static explicit operator {{taskType}}<CancellationToken>(ReplacingAwaitable value) => {{taskType}}.FromResult(CancellationToken.None);
            }
            """;
        await Verify($"cancellationToken = await {expression};", true, extra);
        await Verify("", true, extra, $"await {expression}");
    }

    [TestMethod]
    [DataRow("Task", false)]
    [DataRow("ValueTask", false)]
    [DataRow("Task", true)]
    [DataRow("ValueTask", true)]
    public async Task Helper_Return_Conversions_Do_Not_Prove_Payload(string taskType, bool replaced)
    {
        var payload = replaced
            ? $"({taskType}<CancellationToken>)(ReplacingAwaitable){taskType}.FromResult(token)"
            : $"{taskType}.FromResult(token)";
        var extra = $$"""
            public static class Helpers
            {
                public static {{taskType}}<CancellationToken> Wrap(CancellationToken token) => {{payload}};
            }
            public struct ReplacingAwaitable
            {
                public static explicit operator ReplacingAwaitable({{taskType}}<CancellationToken> value) => new();
                public static explicit operator {{taskType}}<CancellationToken>(ReplacingAwaitable value) => {{taskType}}.FromResult(CancellationToken.None);
            }
            """;
        await Verify("", replaced, extra, "await Helpers.Wrap(cancellationToken)");
    }

    [TestMethod]
    [DataRow("Task")]
    [DataRow("ValueTask")]
    public async Task ConfigureAwait_Evaluates_Receiver_Before_Arguments(string taskType)
    {
        await Verify($"cancellationToken = await {taskType}.FromResult(cancellationToken).ConfigureAwait((cancellationToken = default) == default);", false);
    }

    [TestMethod]
    [DataRow("Task", false)]
    [DataRow("ValueTask", false)]
    [DataRow("Task", true)]
    [DataRow("ValueTask", true)]
    public async Task Payload_Provenance_Does_Not_Suppress_Blocking_Diagnostics(string taskType, bool replaced)
    {
        var payload = replaced ? "CancellationToken.None" : "cancellationToken";
        var call = replaced ? "{|#1:Task.Delay(1, cancellationToken)|}" : "Task.Delay(1, cancellationToken)";
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeader}}
            public class BuildModule : Module<List<string>>
            {
                protected override async Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                {
                    var saved = {{taskType}}.FromResult({{payload}});
                    cancellationToken = {|#0:saved.Result|};
                    await {{call}};
                    return new List<string>();
                }
            }
            """;
        var blocking = VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.BlockingCallId).WithLocation(0).WithArguments("Result");
        await VerifyCS.VerifyAnalyzerAsync(source, replaced
            ? [blocking, VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(1).WithArguments("Delay")]
            : [blocking]);
    }

    private static Task Verify(string statements, bool warns, string extra = "", string token = "cancellationToken")
    {
        var call = warns ? "{|#0:Task.Delay(1, " + token + ")|}" : "Task.Delay(1, " + token + ")";
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeader}}
            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
                protected override async Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                {
                    {{statements}}
                    await {{call}};
                }
            }
            {{extra}}
            """;
        return VerifyCS.VerifyAnalyzerAsync(source, warns
            ? [VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay")]
            : []);
    }
}

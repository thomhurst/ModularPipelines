using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleCancellationTokenControlFlowAnalyzerTests
{
    public static IEnumerable<object[]> LoopCases()
    {
        foreach (var token in new[] { "token", "cancellationToken" })
        {
            foreach (var exit in new[] { "break;", "return null!;", "throw new System.Exception();" })
            {
                yield return ["exclusive-" + token + exit, $$"""
                    var original = cancellationToken;
                    var token = cancellationToken;
                    for (var i = 0; i < 2; i++)
                    {
                        if (context is null) {{token}} = CancellationToken.None;
                        else await CALL(1, {{token}});
                        {{token}} = original;
                        {{exit}}
                    }
                    """, false];
                yield return ["opposite-tail-" + token + exit, $$"""
                    var original = cancellationToken;
                    var token = cancellationToken;
                    for (var i = 0; i < 2; i++)
                    {
                        if (context is null) { {{token}} = CancellationToken.None; {{exit}} }
                        else { await CALL(1, {{token}}); {{token}} = original; }
                    }
                    """, false];
            }

            yield return ["real-back-edge-" + token, $$"""
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    if (context is null) {{token}} = CancellationToken.None;
                    else await CALL(1, {{token}});
                }
                """, true];
            yield return ["continue-" + token, $$"""
                var original = cancellationToken;
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    if (context is null) { {{token}} = CancellationToken.None; continue; }
                    else await CALL(1, {{token}});
                    {{token}} = original;
                }
                """, true];
            yield return ["head-reset-" + token, $$"""
                var original = cancellationToken;
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    {{token}} = original;
                    if (context is null) {{token}} = CancellationToken.None;
                    else await CALL(1, {{token}});
                }
                """, false];
            yield return ["tail-reset-" + token, $$"""
                var original = cancellationToken;
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    if (context is null) {{token}} = CancellationToken.None;
                    else await CALL(1, {{token}});
                    {{token}} = original;
                }
                """, false];
            yield return ["self-reset-" + token, $$"""
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    {{token}} = {{token}};
                    if (context is null) {{token}} = CancellationToken.None;
                    else await CALL(1, {{token}});
                }
                """, true];
            yield return ["conditional-exit-" + token, $$"""
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    if (context is null) {{token}} = CancellationToken.None;
                    else await CALL(1, {{token}});
                    if (context is null) break;
                }
                """, true];
            yield return ["nested-break-" + token, $$"""
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    if (context is null) { {{token}} = CancellationToken.None; do { break; } while (true); }
                    else await CALL(1, {{token}});
                }
                """, true];
            yield return ["nested-continue-outer-break-" + token, $$"""
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    if (context is null) { {{token}} = CancellationToken.None; for (var j = 0; j < 2; j++) continue; }
                    else await CALL(1, {{token}});
                    break;
                }
                """, false];
            yield return ["outer-carry-" + token, $$"""
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    for (var j = 0; j < 2; j++)
                    {
                        if (context is null) {{token}} = CancellationToken.None;
                        else await CALL(1, {{token}});
                        break;
                    }
                }
                """, true];
        }

        foreach (var dependency in new[] { "cancellationToken", "input" })
        {
            yield return ["do-condition-" + dependency, $$"""
                var input = cancellationToken;
                var token = cancellationToken;
                do
                {
                    token = {{dependency}};
                    if (context is null) token = CancellationToken.None;
                    else await CALL(1, token);
                } while (({{dependency}} = CancellationToken.None).CanBeCanceled);
                """, true];
            yield return ["for-iterator-" + dependency, $$"""
                var input = cancellationToken;
                var token = cancellationToken;
                for (var i = 0; i < 2; i++, {{dependency}} = token)
                {
                    token = {{dependency}};
                    if (context is null) token = CancellationToken.None;
                    else await CALL(1, token);
                }
                """, true];
            yield return ["for-initializer-" + dependency, $$"""
                var original = cancellationToken;
                var input = cancellationToken;
                var token = cancellationToken;
                for ({{dependency}} = original; context is not null;)
                {
                    token = {{dependency}};
                    if (context is null) token = CancellationToken.None;
                    else await CALL(1, token);
                }
                """, false];
            yield return ["independent-refresh-" + dependency, $$"""
                var original = cancellationToken;
                var input = cancellationToken;
                var token = cancellationToken;
                for (var i = 0; i < 2; i++, {{dependency}} = token)
                {
                    {{dependency}} = original;
                    token = {{dependency}};
                    if (context is null) token = CancellationToken.None;
                    else await CALL(1, token);
                }
                """, false];
        }

        foreach (var expression in new[] { "Pass(cancellationToken)", "cancellationToken.PassToken()", "Pass(Pass(cancellationToken))", "TokenExtensions.PassToken(cancellationToken)", "Pick(default, cancellationToken)", "Pick(token: cancellationToken, ignored: default)", "LoopPass(cancellationToken)" })
        {
            yield return ["helper-result-" + expression, $$"""
                var token = {{expression}};
                for (var i = 0; i < 2; i++) await CALL(1, token);
                """, false];
            yield return ["helper-reset-" + expression, $$"""
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    cancellationToken = default;
                    token = {{expression}};
                    await CALL(1, token);
                }
                """, true];
        }

        yield return ["helper-discards-parameter", """
            var token = Drop(cancellationToken);
            for (var i = 0; i < 2; i++) await CALL(1, token);
            """, true];
        yield return ["helper-restores-parameter", """
            var token = Restore(cancellationToken);
            for (var i = 0; i < 2; i++) await CALL(1, token);
            """, false];
        yield return ["recursive-helper-swaps-inputs", """
            var token = Swap(cancellationToken, default, 1);
            for (var i = 0; i < 2; i++) await CALL(1, token);
            """, true];
        yield return ["recursive-helper-discards-input", """
            var token = RecursiveDrop(cancellationToken, 1);
            for (var i = 0; i < 2; i++) await CALL(1, token);
            """, true];
        yield return ["helper-discards-local-copy", """
            var token = DropCopy(cancellationToken);
            for (var i = 0; i < 2; i++) await CALL(1, token);
            """, true];

        yield return ["forty-conditional-copies", string.Join("\n", Enumerable.Range(0, 40).Select(i =>
            $"var copy{i} = context is null ? {(i == 0 ? "cancellationToken" : $"copy{i - 1}")} : {(i == 0 ? "cancellationToken" : $"copy{i - 1}")};"))
            + "for (var i = 0; i < 2; i++) await CALL(1, copy39);", false];
        yield return ["linked-source-in-loop", """
            for (var i = 0; i < 2; i++)
            {
                using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                var token = linked.Token;
                await CALL(1, token);
            }
            """, false];
        yield return ["conditional-snapshot-safe", """
            var snapshot = context is null ? cancellationToken : cancellationToken;
            var token = cancellationToken;
            for (var i = 0; i < 2; i++)
            {
                token = snapshot;
                if (context is null) token = CancellationToken.None;
                else await CALL(1, token);
            }
            """, false];
        yield return ["conditional-snapshot-unsafe", """
            var snapshot = context is null ? cancellationToken : CancellationToken.None;
            for (var i = 0; i < 2; i++) await CALL(1, snapshot);
            """, true];
    }

    [TestMethod]
    [DynamicData(nameof(LoopCases))]
    public async Task Tracks_Loop_Reaching_Values(string name, string body, bool warning)
    {
        _ = name;
        await Verify(body, warning);
    }

    public static IEnumerable<object[]> ExceptionCases()
    {
        foreach (var expression in new[] { "target.Run()", "target.Value", "target[0]", "(int)target", "target + 1", "-target" })
        {
            yield return ["dynamic-throw-" + expression, $$"""
                var token = cancellationToken;
                dynamic target = context;
                try { var result = {{expression}}; }
                catch { token = default; }
                await CALL(1, token);
                """, true];
        }

        foreach (var modifier in new[] { "ref", "out" })
        {
            yield return ["dynamic-write-" + modifier, $$"""
                var token = cancellationToken;
                dynamic target = context;
                try { target.Run({{modifier}} token); }
                catch { }
                await CALL(1, token);
                """, true];
        }

        yield return ["unboxing-throw", """
            var token = cancellationToken;
            object boxed = context;
            try { var number = (int)boxed; }
            catch { token = default; }
            await CALL(1, token);
            """, true];
        foreach (var filter in new[] { "(token = default) is var ignored && context is null", "(token = default).CanBeCanceled", "(token = default) is var ignored && context.ToString() is null" })
        {
            yield return ["false-filter-writes-" + filter, $$"""
                var token = cancellationToken;
                try { throw new System.Exception(); }
                catch (System.Exception) when ({{filter}}) { return null!; }
                catch { }
                await CALL(1, token);
                """, true];
        }

        yield return ["filter-writes-reach-outer-handler", """
            var token = cancellationToken;
            try
            {
                try { throw new System.Exception(); }
                catch (System.Exception) when ((token = default) is var ignored && context is null) { return null!; }
            }
            catch { }
            await CALL(1, token);
            """, true];
        yield return ["true-filter-does-not-reach-sibling", """
            var token = cancellationToken;
            try { throw new System.Exception(); }
            catch (System.Exception) when (true) { token = cancellationToken; }
            catch { token = default; }
            await CALL(1, token);
            """, false];

        yield return ["typed-true-filter-can-be-skipped", """
            var token = cancellationToken;
            try { throw new System.Exception(); }
            catch (System.InvalidOperationException) when (true) { return null!; }
            catch { token = default; }
            await CALL(1, token);
            """, true];
        yield return ["discard-does-not-throw", """
            var token = cancellationToken;
            try { _ = context; }
            catch { token = default; }
            await CALL(1, token);
            """, false];
        yield return ["throwing-filter-retains-writes", """
            var token = cancellationToken;
            try { throw new System.Exception(); }
            catch (System.Exception) when ((token = default) is var ignored && ThrowFilter()) { return null!; }
            catch { }
            await CALL(1, token);
            """, true];
        yield return ["false-filter-restores", """
            var token = default(CancellationToken);
            try { throw new System.Exception(); }
            catch (System.Exception) when ((token = cancellationToken) is var ignored && false) { return null!; }
            catch { }
            await CALL(1, token);
            """, false];
        yield return ["recursive-loop-helper-is-bounded", """
            var token = RecursivePass(cancellationToken, 2);
            for (var i = 0; i < 2; i++) await CALL(1, token);
            """, false];

        yield return ["return-does-not-throw", """
            var token = cancellationToken;
            try { if (context is null) { token = default; return null!; } }
            catch { }
            await CALL(1, token);
            """, false];
        yield return ["throwing-path", """
            var token = cancellationToken;
            try { token = default; throw new System.Exception(); }
            catch { }
            await CALL(1, token);
            """, true];
        yield return ["catch-restores", """
            var token = cancellationToken;
            try { token = default; throw new System.Exception(); }
            catch { token = cancellationToken; }
            await CALL(1, token);
            """, false];
        yield return ["sibling-rethrow", """
            var token = cancellationToken;
            try { if (context is null) { token = default; throw new System.InvalidOperationException(); } }
            catch (System.InvalidOperationException) { throw; }
            catch { }
            await CALL(1, token);
            """, true];
        yield return ["finally-restores", """
            var token = cancellationToken;
            try { token = default; }
            finally { token = cancellationToken; }
            await CALL(1, token);
            """, false];
        yield return ["finally-throws-to-catch", """
            var token = cancellationToken;
            try { try { token = default; return null!; } finally { throw new System.Exception(); } }
            catch { }
            await CALL(1, token);
            """, true];
        yield return ["sibling-catch-rethrow-does-not-continue", """
            var token = cancellationToken;
            try { _ = context.ToString(); }
            catch (System.InvalidOperationException) { token = default; throw; }
            catch { }
            await CALL(1, token);
            """, false];
        yield return ["nested-catch-restores-before-outer", """
            var token = cancellationToken;
            try
            {
                try { token = default; throw new System.Exception(); }
                catch { token = cancellationToken; }
            }
            catch { }
            await CALL(1, token);
            """, false];
        yield return ["nested-finally-restores-before-catch", """
            var token = cancellationToken;
            try
            {
                try { token = default; throw new System.Exception(); }
                finally { token = cancellationToken; }
            }
            catch { }
            await CALL(1, token);
            """, false];
        foreach (var cleanup in new[] { "using (var resource = new Resource())", "await using (var resource = new Resource())",
            "foreach (var item in new Sequence())", "await foreach (var item in new Sequence())" })
        {
            foreach (var handler in new[] { "", "return null!;", "token = cancellationToken;" })
            {
                yield return [cleanup + handler, $$"""
                    var token = cancellationToken;
                    try { {{cleanup}} { token = default; return null!; } }
                    catch { {{handler}} }
                    await CALL(1, token);
                    """, handler.Length == 0];
            }
        }

        foreach (var collection in new[] { "new[] { 1 }", "\"text\"" })
        {
            yield return ["no-dispose-" + collection, $$"""
                var token = cancellationToken;
                try { foreach (var item in {{collection}}) { token = default; return null!; } }
                catch { }
                await CALL(1, token);
                """, false];
        }
    }

    [TestMethod]
    [DynamicData(nameof(ExceptionCases))]
    public async Task Tracks_Exception_And_Cleanup_Values(string name, string body, bool warning)
    {
        _ = name;
        await Verify(body, warning);
    }

    private static async Task Verify(string body, bool warning)
    {
        body = body.Replace("CALL", warning ? "{|#0:Task.Delay|}" : "Task.Delay");
        // Mark the invocation, rather than only its method name.
        body = body.Replace("{|#0:Task.Delay|}(1, token)", "{|#0:Task.Delay(1, token)|}")
            .Replace("{|#0:Task.Delay|}(1, cancellationToken)", "{|#0:Task.Delay(1, cancellationToken)|}")
            .Replace("{|#0:Task.Delay|}(1, snapshot)", "{|#0:Task.Delay(1, snapshot)|}")
            .Replace("{|#0:Task.Delay|}(1, copy39)", "{|#0:Task.Delay(1, copy39)|}");
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeaderWithExtensions}}
            public class BuildModule : Module<List<string>>
            {
                private static CancellationToken Pass(CancellationToken token) => token;
                private static CancellationToken Pick(CancellationToken ignored, CancellationToken token) => token;
                private static bool ThrowFilter() => throw new System.Exception();
                private static CancellationToken LoopPass(CancellationToken token)
                {
                    for (var i = 0; i < 2; i++) { }
                    return token;
                }
                private static CancellationToken Drop(CancellationToken token)
                {
                    for (var i = 0; i < 2; i++) { }
                    token = default;
                    return token;
                }
                private static CancellationToken Restore(CancellationToken token)
                {
                    var original = token;
                    for (var i = 0; i < 2; i++) { }
                    token = default;
                    token = original;
                    return token;
                }
                private static CancellationToken DropCopy(CancellationToken token)
                {
                    var copy = token;
                    for (var i = 0; i < 2; i++) { }
                    copy = default;
                    return copy;
                }
                private static CancellationToken Swap(CancellationToken token, CancellationToken other, int remaining)
                {
                    for (var i = 0; i < remaining; i++) token = Swap(other, token, remaining - 1);
                    return token;
                }
                private static CancellationToken RecursiveDrop(CancellationToken token, int remaining)
                {
                    for (var i = 0; i < remaining; i++) token = RecursiveDrop(token, remaining - 1);
                    token = default;
                    return token;
                }
                private static CancellationToken RecursivePass(CancellationToken token, int remaining)
                {
                    for (var i = 0; i < remaining; i++) token = RecursivePass(token, remaining - 1);
                    return token;
                }

                protected override async Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                {
                    {{body}}
                    return null!;
                }
            }
            public static class TokenExtensions
            {
                public static CancellationToken PassToken(this CancellationToken token) => token;
            }
            public class Resource : System.IDisposable, System.IAsyncDisposable
            {
                public void Dispose() => throw new System.Exception();
                public System.Threading.Tasks.ValueTask DisposeAsync() => throw new System.Exception();
            }
            public class Sequence : System.Collections.Generic.IEnumerable<int>, System.Collections.Generic.IAsyncEnumerable<int>
            {
                public System.Collections.Generic.IEnumerator<int> GetEnumerator() => throw new System.Exception();
                System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
                public System.Collections.Generic.IAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = default) => throw new System.Exception();
            }
            """;
        if (warning)
        {
            await VerifyCS.VerifyAnalyzerAsync(source, VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
        }
        else
        {
            await VerifyCS.VerifyAnalyzerAsync(source);
        }
    }
}

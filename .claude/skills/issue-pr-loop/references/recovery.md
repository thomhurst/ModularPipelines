# Recover preserved merged-worktree changes

Use this procedure only when a cleanup survey preserves a merged-PR worktree with potentially valuable changes. Prioritize actionable open PRs first. Recovery is not permission to discard another agent's work.

1. Verify the original merged PR identity and acquire `pr-<original-N>` through the canonical lock script. Confirm no active agent, open PR, or harness owns the worktree; otherwise leave it untouched.
2. Compare tracked changes, untracked source, and local-only commits against both the original PR and fresh `origin/main`. Identify coherent, unique source/test changes separately from obsolete or generated churn. Search for an existing recovery PR/issue before creating another.
3. Preserve mixed, unclear, sensitive, or untestable changes. When authorized, record a recovery issue with the original PR, worktree path/branch, and a non-sensitive description; do not publish secrets or speculative changes.
4. Before modifying or removing the old worktree, preserve its exact valuable edits and local-only commits in a local snapshot branch/commit or another verified backup. Do not blindly stage secrets, unrelated user files, or generated artifacts.
5. Create a separate worktree from current `origin/main` and transplant only the reviewed diff. Publishing the old squash-merged branch can replay already-merged history. Split unrelated edits, omit already-merged changes, and regenerate generated files from their sources.
6. Validate the recovered change under the repository's normal requirements, review the complete diff, and open a ready-for-review PR with `Follow-up to #<original-N>` explaining the recovered intent. Confirm the remote PR head matches local HEAD.
7. Remove the old worktree/snapshot only after every valuable change is accounted for in the confirmed PR or retained backup, the path is verified, and repository cleanup rules permit removal. Otherwise preserve them. Release ownership in every outcome.

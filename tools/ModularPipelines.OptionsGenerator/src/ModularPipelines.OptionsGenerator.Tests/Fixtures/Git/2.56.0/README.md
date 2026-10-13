# Git 2.56.0 help fixtures

These captured Git for Windows fixtures are recovered from `issue-5731-git-globals`
at `bb0d095746ff59f1763816fab9b45aac9e1932cb`, preserved after PR #5750 closed.
`version.txt` records the executable version. `root-help.txt`, `apply-help.txt`,
and `branch-help.txt` contain `git -h`, `git apply -h`, and `git branch -h` output.

The branch fixture advertises repeatable `--delete-merged` and `--forked` values.
The root and apply fixtures exercise the different meanings of root and local `-C`.
Fixtures do not replace Linux command coverage or generator provenance. Refresh those
only through the complete Generate CLI Options workflow on the source branch.

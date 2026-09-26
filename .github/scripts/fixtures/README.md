# Review execution fixture

`claude-review-success.json` is the unchanged `validated-review.json` artifact from
[workflow run 36211943253](https://github.com/thomhurst/ModularPipelines/actions/runs/36211943253),
artifact 10896525101, reviewing commit `bceffd9d281528b295a439ac482e473c1e309a6e`
for PR #5381 on 2026-09-26.

The workflow captured the action's actual final response only after successful
validation and publication as review 5324237354. It retained the result envelope
fields used by the publisher and omitted intermediate messages, credentials,
session identifiers, and other private metadata. The response includes the actual
Markdown JSON fence that caused earlier raw-JSON parsing failures. Review notes
describe the state at capture time; they are fixture data, not current findings.

SHA-256 of the downloaded fixture:
`8fbbb8a84e2231e04cfce1628fac04a956f499d3f2ed0be209bc1d818b3d5639`.

The command-line regression supplies the original changed-file paths and mocks
only the `gh` process boundary with `mock-review-github.mjs`. Filesystem reads,
execution extraction, JSON parsing, contract validation, and rendering use the
production entry point. A mismatched changed-file list must still prevent any
GitHub call.

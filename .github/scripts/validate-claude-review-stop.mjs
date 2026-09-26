import { readFileSync } from 'node:fs';
import { buildReview } from './post-claude-review.mjs';

try {
  const event = JSON.parse(readFileSync(0, 'utf8'));
  if (event?.hook_event_name !== 'Stop' || typeof event.stop_hook_active !== 'boolean'
    || typeof event.last_assistant_message !== 'string') {
    throw new Error('Invalid Stop event.');
  }

  let decision = {};
  // Never loop on an invalid response. The independent publisher still rejects
  // invalid output after this single in-session correction opportunity.
  if (!event.stop_hook_active) {
    const changedFiles = readFileSync(process.env.REVIEW_CHANGED_FILES, 'utf8').split('\0').filter(Boolean);
    try {
      buildReview(event.last_assistant_message, process.env.REVIEW_HEAD_SHA, changedFiles);
    } catch (error) {
      // buildReview emits fixed diagnostics, never source or model text. Reuse
      // its full contract instead of accepting a weaker JSON-only check here.
      decision = {
        decision: 'block',
        reason: `${error.message} Return the complete actual review as one JSON object with exactly summary, findings, notes, and evidence, following the review contract. Preserve every actionable finding and concrete evidence; do not substitute an acknowledgment, example, or empty review.`,
      };
    }
  }
  console.log(JSON.stringify(decision));
} catch {
  // Input and filesystem errors can contain private content or paths.
  console.error('Claude review Stop hook could not validate its input.');
  process.exitCode = 1;
}

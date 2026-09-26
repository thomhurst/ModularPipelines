import { readFileSync } from 'node:fs';
import { pathToFileURL } from 'node:url';

export function classifyFailure(rawExecution) {
  let messages;
  try {
    messages = JSON.parse(rawExecution);
  } catch {
    return 'unavailable execution';
  }
  const result = Array.isArray(messages) ? messages.at(-1) : undefined;
  if (result?.type !== 'result' || result.is_error !== true) {
    return 'unclassified failure';
  }

  // Inspect only the failed terminal result. Return fixed categories, never
  // error text, transcript content, account details, or credentials.
  const detail = [result.result, ...(Array.isArray(result.errors) ? result.errors : [])]
    .filter(value => typeof value === 'string').join('\n');
  if (/rate.?limit|usage.?limit|hit your limit|insufficient_quota|credit balance/i.test(detail)) {
    return 'rate or usage limit';
  }
  if (/authentication_error|unauthorized|invalid.{0,20}(?:token|api.key)|(?:token|oauth).{0,20}expired/i.test(detail)) {
    return 'authentication failure';
  }
  if (/overloaded_error|service unavailable|connection (?:error|refused|reset)|ECONNRESET|ETIMEDOUT/i.test(detail)) {
    return 'service or connection failure';
  }
  return 'unclassified model error';
}

if (process.argv[1] && import.meta.url === pathToFileURL(process.argv[1]).href) {
  let category = 'unavailable execution';
  try {
    category = classifyFailure(readFileSync(process.env.REVIEW_EXECUTION_FILE, 'utf8'));
  } catch {
    // The action can fail before writing its execution file.
  }
  console.log(`Claude execution failure category: ${category}.`);
}

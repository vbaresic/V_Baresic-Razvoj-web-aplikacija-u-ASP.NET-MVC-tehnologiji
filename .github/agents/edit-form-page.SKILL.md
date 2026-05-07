---
name: edit-form-page
description: Creates or modifies Create/Edit form pages for entities, including input validation and submission handling.
---

This skill is invoked when the user needs to:
- Create new Create or Edit views with forms.
- Add POST actions for form submission.
- Implement input validation and error handling.
- Use HTML helpers like EditorFor, TextBoxFor, or tag helpers.

When using this skill:
- Use [HttpGet] for display, [HttpPost] for submission.
- Bind models with [Bind] attributes for security.
- Redirect to Index or Details after success.
- Include validation summaries and anti-forgery tokens.
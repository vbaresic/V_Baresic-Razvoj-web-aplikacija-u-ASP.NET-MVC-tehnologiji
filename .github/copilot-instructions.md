# Main Agent Instructions

## Project Context
This is an ASP.NET Core MVC application built with Razor Views.
It uses a mock repository with static data (no database).

## Sub-Agent Delegation
This project uses specialized sub-agents defined in `.github/agents/`.

### Main-Agent First Gate
- The main agent MUST always send one user-visible response first before invoking any sub-agent.
- That first response must include the `Logging Protocol` start block with the full original user prompt.
- Sub-agent delegation is allowed only after that initial main-agent response has been emitted.

### UX Sub-Agent Handoff
**Agent**: `.github/agents/ux-sub-agent.agent.md` (name: "UX Sub-Agent")
**Trigger**: Delegate to UX Sub-Agent when:
- User asks to generate or modify any `.cshtml` view file
- User asks to write or modify CSS in `/wwwroot/css/`
- User asks to create layout or partial view files
- User asks to build navigation, cards, lists, detail pages, or any UI components
- User explicitly requests "UX" or "design system" work

When delegating UI code requests, invoke via `@UX Sub-Agent` and log the handoff.
Example delegation:
> "[SUB-AGENT INVOKED] UX Sub-Agent — applying design system from ux-sub-agent.agent.md"

## General Rules
- All data comes from mock repository classes (static lists, no EF Core, no database).
- No Create/Edit pages — only Index (list) and Details pages per entity.
- Navigation must be complete: menu, list-to-details links, breadcrumbs.

## Logging Protocol
- When you start processing any request, begin your response with:
  [MAIN-AGENT START] {brief description of request}
  **User Prompt:** "{full user prompt as literally written}"
- When you delegate to ANY sub-agent, write:
  [SUB-AGENT INVOKED] {agent name} — **Original Prompt:** "{full user prompt}"
  **Reason:** {delegation reason}
- When a sub-agent finishes, write:
  [SUB-AGENT DONE] {agent name} | Files: {files affected}
- When returning to main context, write:
  [MAIN-AGENT RESUME]

**IMPORTANT:** Always log the complete user prompt exactly as provided, even when delegating. This ensures all decisions are traceable to the original request.
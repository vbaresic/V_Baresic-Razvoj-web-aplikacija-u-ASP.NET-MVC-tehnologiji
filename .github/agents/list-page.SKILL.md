---
name: list-page
description: Creates or modifies list/index pages for entities, displaying collections of data with navigation and cards.
---

This skill is invoked when the user needs to:
- Create a new Index (list) view for an entity.
- Modify existing list pages to add features like sorting, filtering, or pagination.
- Generate controller actions that return lists of entities.
- Set up breadcrumbs and navigation for list pages.

When using this skill:
- Use EF to query the database asynchronously.
- Create EntityCardViewModel for each item.
- Include links to Details pages.
- Follow the existing design system with content-card and entity-list classes.
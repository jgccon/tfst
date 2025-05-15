---
id: organizations-team-management
title: Organizations Team Management
---

# Organizations & Team Management

## Overview
Supports companies and agencies in managing their teams, job postings, and talent acquisitions.

## Core Entities
- **Organization**: Represents a company or agency hiring freelancers.
- **User Organization Membership**: A link between users and organizations.
- **Recruiter**: A specialized user role within an organization.

## Relationships
- **User (0:N) Organizations**: A user can belong to multiple organizations.
- **Organization (1:N) Job Posts**: Companies post multiple jobs.

```mermaid
erDiagram
    User ||--o{ Organization : belongs_to
    Organization ||--o{ JobPost : creates
    Organization ||--o{ UserOrganizationMembership : manages
    UserOrganizationMembership ||--|| User : links
```

## Key Features
- Multi-user organization management.
- Role-based access for recruiters and HR teams.
- Centralized dashboard for tracking job posts and hires.

## Future Enhancements
- AI-driven hiring insights and team recommendations.

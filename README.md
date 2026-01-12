#  Studying Plan

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Status](https://img.shields.io/badge/Status-In%20Development-yellow?style=for-the-badge)

##  Description
**Studying Plan** is an academic planning tool designed to help students manage coursework deadlines effectively. It uses **ASP.NET Core MVC** and **SQL Server** to generate custom study schedules and visualize workload weight, ensuring students can prioritize the right tasks at the right time.

## Tech Stack

### Main Application (Server-Side Rendering)
* **Framework:** ASP.NET Core MVC
* **Database:** SQL Server
    * *Includes:* Essential Views and Stored Procedures for CRUD operations on Assignments, Courses, and Weekly Schedules.

### Browser Extension (Integration)
* **Purpose:** Extracts data from D2L-based university websites (e.g., eConestoga) to automate data entry.
* **Technologies:** REST API, ASP.NET Web API.

---

##  Milestones & Features

### 1. Website (Core Platform)
* **1.1. User Management**
    * - [x] Login/Register Feature
    * - [ ] *[Planned]* Advanced Auth (Forgot Password, Email Confirmation)
* **1.2. Academic Management**
    * - [x] Essential CRUD operations for Courses and Assignments
* **1.3. Visualization & Notifications**
    * - [x] Assignment Calendar & Stack View (Weighted view to prioritize tasks by deadlines/importance)
    * - [ ] *[In Progress]* Notification System
* **1.4. Smart Features**
    * - [ ] *[Planned]* Custom Study Plan Generator (AI-assisted or User-defined)
* **1.5. Collaboration**
    * - [ ] *[Researching]* Group Study & Chat (exploring WebSocket/SignalR)

### 2. Browser Extension (D2L Scraper)
* **Goal:** Automatically import essential information (Assignments, Courses, Due Dates) from D2L to the main website.
* **2.1. Development Status**
    * - [ ] *[In Progress]* Basic Operations and Data Transfer

---

##  Current Focus
1.  **Notification System:** Implementing alerts for upcoming deadlines.
2.  **Chrome Extension:** Building the data extractor for D2L pages.

## Deployment
* **Live Link:** [Not Deploy Yet]

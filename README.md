# 📑 General Ledger (G/L) Transactions Listing Report Application

An enterprise-grade **General Ledger (G/L) Reporting & Financial Analytics Application** built using **VB.NET (WinForms)**, **SAP Crystal Reports**, and **Microsoft SQL Server (T-SQL)**.

This solution allows finance teams and auditors to generate detailed G/L transaction listings formatted in functional currency. It provides dynamic account lookup modals, account segment filters, running balance calculations per period/fiscal year, and module-level filtering (AP, AR, BK, GL, IC, OE, PO).

---

## 🚀 Key Features

* **Dynamic Running Balances**: Calculates accurate `OPENING_BALANCE` and `CLOSING_BALANCE` per account dynamically across fiscal years and periods using SQL Window functions (`SUM() OVER (PARTITION BY ... ORDER BY ...)`).
* **Multi-Segment Search & Filtering**: Filter report data by Date Range, Fiscal Year/Period, Account Range, Sub-ledger Segments (Accounts, Entity, Concern, etc.), and Source Modules.
* **Interactive Account Search Modal**: Features dedicated lookup dialogs (`Search.vb` & `SearchSegment.vb`) allowing users to easily search, filter, and select GL Account IDs and Segments.
* **Multi-Module Source Integration**: Consolidates transactional postings across enterprise sub-modules including:
  * **AP** (Accounts Payable)
  * **AR** (Accounts Receivable)
  * **BK** (Bank Services)
  * **GL** (General Ledger)
  * **IC** (Inventory Control)
  * **OE** (Order Entry)
  * **PO** (Purchase Orders)
* **Drill-Down & Source Tracking**: Features built-in parsing logic (`DRILLDWNLK`) to extract and render precise `Source_Batch_No`, `Source_Item_No`, `GL Batch`, and `Check/Rpt No.` references for seamless auditing.
* **Color-Coded Module Indicators**: Embedded visual legends within Crystal Reports for fast source identification (GL, AP, AR, IC, PO, OE, BK, Others).

---

## 🛠️ Tech Stack & Architecture

* **UI & Frontend Engine**: Visual Basic .NET (WinForms / .NET Framework 4.8)
* **Reporting Framework**: SAP Crystal Reports (`CrystalDecisions.CrystalReports.Engine`)
* **Database Engine**: Microsoft SQL Server (T-SQL Views, Windowed Functions, Joins)
* **IDE**: Microsoft Visual Studio 2022

---

## 📷 Application UI & Report Previews

### 1. Main Parameter Control Form
The primary entry form used by operators to configure report parameters, date ranges, and target source modules.

![Main Parameter Control Form]<img width="1693" height="929" alt="General-Ledger-Report" src="https://github.com/user-attachments/assets/311a7d7e-34ae-4e63-87cc-7fcd7b1d3b2f" />


### 2. Interactive Account Search Modal
Dedicated lookup window allowing accounting personnel to search by account codes, descriptions, and segment filters.

![Interactive Account Search Modal]<img width="1600" height="656" alt="General-Ledger-Report1" src="https://github.com/user-attachments/assets/d188b6f0-2cf9-4219-ab42-dcbd4b8c15de" />


### 3. General Ledger Transaction Listing - Multi-Period View
Sample output showing multi-period roll-forwards, account group headers, monthly summary totals, net changes, and source module tags.

![Multi-Period Report View]<img width="935" height="565" alt="General-Ledger-Report2" src="https://github.com/user-attachments/assets/825fcf2d-b813-4c7d-8bc2-51dc37e16d34" />


### 4. General Ledger Transaction Listing - Summary & Totals
Detailed bottom-line rendering featuring period net changes, fiscal year balances, overall account totals, and the source module color legend.

![Report Summary and Totals]<img width="937" height="556" alt="General-Ledger-Report3" src="https://github.com/user-attachments/assets/493d5dbc-8d39-4b87-bedf-240d2d3457f6" />


---

## 🗄️ Database View Schema (`VIEW_GL_TRANSACTION_LISTING`)

Below is the complete T-SQL script powering the data layer:

```sql
USE BNBDAT;
GO

CREATE OR ALTER VIEW VIEW_GL_TRANSACTION_LISTING AS
SELECT
    MF.ACCTID,
    MF.ACSEGVAL01 AS Accounts,
    MF.ACCTFMTTD AS [FORMATTED ACCOOUNTS],
    MF.ACSEGVAL02 AS Entity,
    MF.ACSEGVAL03 AS Concern,
    MF.ACSEGVAL04 AS Segment03,
    MF.ACSEGVAL05 AS Segment04,
    MF.ACSEGVAL06 AS Segment05,
    MF.ACSEGVAL07 AS Segment06,
    MF.ACSEGVAL08 AS Segment07,
    MF.ACSEGVAL09 AS Segment08,
    MF.ACSEGVAL10 AS Segment09,

    CASE
        WHEN ACTIVESW = 1 THEN 'Active'
        ELSE 'Inactive'
    END AS Status,

    MF.ABRKID AS [Structure Code],

    -- Account Type
    CASE MF.ACCTTYPE
        WHEN 'B' THEN 'BS'
        WHEN 'I' THEN 'IS'
        WHEN 'R' THEN 'RE'
        ELSE 'Unknown'
    END AS [Account Type],

    -- Normal Balance
    CASE MF.ACCTBAL
        WHEN '1' THEN 'Debit'
        WHEN '2' THEN 'Credit'
        ELSE 'Unknown'
    END AS [Normal Balance],

    MF.ACCTGRPCOD AS [Account Group],

    -- Account Group Description
    G.ACCTGRPDES AS [Account Group Description],

    -- Control Account
    CASE MF.CTRLACCTSW
        WHEN '1' THEN 'Yes'
        WHEN '0' THEN 'No'
        ELSE 'Unknown'
    END AS [Control Account],

    -- Rollup
    CASE MF.ROLLUPSW
        WHEN '1' THEN 'Yes'
        WHEN '0' THEN 'No'
        ELSE 'Unknown'
    END AS [Rollup],

    -- Post to Account
    CASE MF.CONSLDSW
        WHEN '0' THEN 'Detail'
        WHEN '1' THEN 'Consolidated'
        WHEN '2' THEN 'Prohibited'
        ELSE 'Unknown'
    END AS [Post to Account],

    ST.DOCDATE,
    RIGHT('0' + CAST(ST.FISCALPERD AS VARCHAR), 2) AS Prd,
    ST.FISCALYR,
    ST.FISCALPERD,

    MF.ACCTDESC,
    ST.DRILLDWNLK,
    
    -- Source_Batch_No & Source_Item_No Logic
    SUBSTRING(CAST(ST.DRILLDWNLK AS VARCHAR(18)), 7, 4) AS Source_Batch_No,
    SUBSTRING(CAST(ST.DRILLDWNLK AS VARCHAR(18)), 18, 1) AS Source_Item_No,
    
    -- Modified [Source Batch] with NULL/Empty logic
    CASE 
        WHEN ST.DRILLDWNLK IS NULL OR TRIM(CAST(ST.DRILLDWNLK AS VARCHAR(18))) = '' THEN ''
        WHEN TRIM(SUBSTRING(CAST(ST.DRILLDWNLK AS VARCHAR(18)), 7, 4)) = '' 
             AND TRIM(SUBSTRING(CAST(ST.DRILLDWNLK AS VARCHAR(18)), 18, 1)) = '' THEN ''
        WHEN TRIM(SUBSTRING(CAST(ST.DRILLDWNLK AS VARCHAR(18)), 7, 4)) = '' 
            THEN TRIM(SUBSTRING(CAST(ST.DRILLDWNLK AS VARCHAR(18)), 18, 1))
        WHEN TRIM(SUBSTRING(CAST(ST.DRILLDWNLK AS VARCHAR(18)), 18, 1)) = '' 
            THEN TRIM(SUBSTRING(CAST(ST.DRILLDWNLK AS VARCHAR(18)), 7, 4))
        ELSE CONCAT(
            SUBSTRING(CAST(ST.DRILLDWNLK AS VARCHAR(18)), 7, 4), 
            '-', 
            SUBSTRING(CAST(ST.DRILLDWNLK AS VARCHAR(18)), 18, 1)
        )
    END AS [Source Batch],

    ST.SRCELEDGER,
    ST.SRCETYPE,
    CONCAT(ST.SRCELEDGER, '-', ST.SRCETYPE) AS SC,
    ST.POSTINGSEQ,
    ST.BATCHNBR,
    ST.ENTRYNBR,
    CONCAT(CAST(ST.BATCHNBR AS INT), '-', CAST(ST.ENTRYNBR AS INT)) AS [GL Batch],
    ST.JNLDTLDESC,
    ST.JNLDTLREF,
    ST.TRANSAMT,
    
    CASE
        WHEN ST.TRANSAMT > 0 THEN ST.TRANSAMT
        ELSE 0
    END AS Debits,
    
    CASE 
        WHEN ST.TRANSAMT <= 0 THEN ABS(ST.TRANSAMT)
        ELSE 0
    END AS Credits,

    -- Dynamic OPENING BALANCE
    ISNULL(
        SUM(ST.TRANSAMT) OVER (
            PARTITION BY MF.ACCTID 
            ORDER BY ST.FISCALYR, ST.FISCALPERD, ST.DOCDATE, ST.POSTINGSEQ
            ROWS BETWEEN UNBOUNDED PRECEDING AND 1 PRECEDING
        ), 0
    ) AS OPENING_BALANCE,

    -- Dynamic CLOSING / RUNNING BALANCE
    SUM(ST.TRANSAMT) OVER (
        PARTITION BY MF.ACCTID 
        ORDER BY ST.FISCALYR, ST.FISCALPERD, ST.DOCDATE, ST.POSTINGSEQ
        ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
    ) AS CLOSING_BALANCE,

    CASE
        WHEN ST.SRCETYPE = 'IN' THEN ''
        ELSE CONCAT(
            'Check/Rpt No.-',
            RIGHT('000000000' + SUBSTRING(CAST(ST.DRILLDWNLK AS VARCHAR(20)), 7, 4), 9),
            '-',
            RIGHT('00000' + CAST(RIGHT(CAST(ST.DRILLDWNLK AS VARCHAR(20)), 10) AS VARCHAR), 5)
        )
    END AS Check_Rpt_No

FROM GLAMF MF
LEFT JOIN GLPOST ST 
    ON MF.ACCTID = ST.ACCTID
LEFT JOIN GLACGRP G
    ON MF.ACCTGRPCOD = G.ACCTGRPCOD;
GO
```

---

## 📁 Repository Structure

```text
General Ledger Report2_Final/
│
├── General Ledger Report2.sln             # Visual Studio Solution File
│
└── General Ledger Report2/                # Core Application Project
    ├── Main Form.vb                       # Primary UI & Parameter Dialog Logic
    ├── Main Form.Designer.vb              # Form UI Designer Code
    ├── Search.vb                          # Account Lookup Modal Logic
    ├── SearchSegment.vb                   # Sub-ledger Segment Search Modal
    ├── ReportViewer.vb                    # SAP Crystal Reports Host Control Window
    │
    ├── SQL/                               # Database View Scripts
    │   ├── General_Ledger.sql
    │   ├── General_Ledger_with_grp_description.sql
    │   └── Transaction_Listing_view.sql
    │
    ├── Report/                            # Crystal Report Formats
    │   └── GL_TRANSACTIONS_LISTING.rpt    # General Ledger Listing Layout Template
    │
    └── My Project/                        # Application Assembly & Resources Configuration
```

---

## ⚙️ Installation & Setup Instructions

1. **Clone Repository**:
   ```powershell
   git clone https://github.com/MdAshikurRahmanShuvo200/General-Ledger-Report.git
   cd "General Ledger Report2_Final"
   ```

2. **Execute Database View**:
   * Open Microsoft SQL Server Management Studio (SSMS).
   * Run `General_Ledger_with_grp_description.sql` on your target database (`BNBDAT`).

3. **Build & Run Application**:
   * Open `General Ledger Report2.sln` in Visual Studio 2022.
   * Verify SAP Crystal Reports runtime engine for .NET is installed.
   * Press `F5` to build and run.

---

## 👨‍💻 Author

**Md. Ashikur Rahman Shuvo**  
*Data Analyst & Software Solutions Developer*  
* GitHub: [@MdAshikurRahmanShuvo200](https://github.com/MdAshikurRahmanShuvo200)

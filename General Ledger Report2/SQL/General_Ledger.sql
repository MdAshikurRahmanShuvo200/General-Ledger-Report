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
        WHEN 'B' THEN 'Balance Sheet'
        WHEN 'I' THEN 'Income Statement'
        WHEN 'R' THEN 'Retained Earnings'
        ELSE 'Unknown'
    END AS [Account Type],

    -- Normal Balance
    CASE MF.ACCTBAL
        WHEN '1' THEN 'Debit'
        WHEN '2' THEN 'Credit'
        ELSE 'Unknown'
    END AS [Normal Balance],

    MF.ACCTGRPCOD AS [Account Group],

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
        WHEN TRIM(SUBSTRING(CAST(ST.DRILLDWNLK AS VARCHAR(18)), 7, 4)) = '' AND TRIM(SUBSTRING(CAST(ST.DRILLDWNLK AS VARCHAR(18)), 18, 1)) = '' THEN ''
        WHEN TRIM(SUBSTRING(CAST(ST.DRILLDWNLK AS VARCHAR(18)), 7, 4)) = '' THEN TRIM(SUBSTRING(CAST(ST.DRILLDWNLK AS VARCHAR(18)), 18, 1))
        WHEN TRIM(SUBSTRING(CAST(ST.DRILLDWNLK AS VARCHAR(18)), 18, 1)) = '' THEN TRIM(SUBSTRING(CAST(ST.DRILLDWNLK AS VARCHAR(18)), 7, 4))
        ELSE CONCAT(SUBSTRING(CAST(ST.DRILLDWNLK AS VARCHAR(18)), 7, 4), '-', SUBSTRING(CAST(ST.DRILLDWNLK AS VARCHAR(18)), 18, 1))
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
    -- ১. ডাইনামিক OPENING BALANCE (নির্দিষ্ট Account-এর বর্তমান ট্রানজাকশনের ঠিক আগের সকল ট্রানজাকশনের Sum)
    ISNULL(
        SUM(ST.TRANSAMT) OVER (
            PARTITION BY MF.ACCTID 
            ORDER BY ST.FISCALYR, ST.FISCALPERD, ST.DOCDATE, ST.POSTINGSEQ
            ROWS BETWEEN UNBOUNDED PRECEDING AND 1 PRECEDING
        ), 0
    ) AS OPENING_BALANCE,

    -- ২. ডাইনামিক CLOSING / RUNNING BALANCE (বর্তমান ট্রানজাকশনসহ মোট জের)
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
    ON MF.ACCTID = ST.ACCTID;
GO

SELECT * FROM VIEW_GL_TRANSACTION_LISTING
ORDER BY ACCTID;
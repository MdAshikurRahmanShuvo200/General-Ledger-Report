Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Windows.Forms

Public Class ReportViewer
    ' Basic Parameter Properties
    Public Property ParamFromAccID As String
    Public Property ParamToAccID As String
    Public Property ParamFromYear As String
    Public Property ParamToYear As String
    Public Property ParamFromPeriod As String
    Public Property ParamToPeriod As String

    ' --- Crystal Report Segment Parameter Properties ---
    Public Property ParamFromAccounts As String
    Public Property ParamToAccounts As String
    Public Property ParamFromEntity As String
    Public Property ParamToEntity As String
    Public Property ParamFromConcern As String
    Public Property ParamToConcern As String
    Public Property ParamFromFormattedAcc As String
    Public Property ParamToFormattedAcc As String

    ' --- Source Modules Parameter Properties ---
    Public Property ParamSourceModulesList As New List(Of String)()
    ' NEW: String property for displaying selected modules on report header
    Public Property ParamSourceModulesDisplay As String

    Private Sub ReportViewer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim rptDoc As New ReportDocument()

            ' 1. Exact Report File Search
            Dim reportFileName As String = "GL_TRANSACTIONS_LISTING.rpt"
            Dim reportPath As String = FindReportFilePath(reportFileName)

            If String.IsNullOrEmpty(reportPath) OrElse Not File.Exists(reportPath) Then
                MessageBox.Show("Report file '" & reportFileName & "' khunje pawa jayni!", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            ' Load Report
            rptDoc.Load(reportPath)

            ' 2. Database Connection Details
            Dim connectionInfo As New ConnectionInfo() With {
                .ServerName = "LAPTOP-8VQAAFE9",
                .DatabaseName = "BNBDAT",
                .UserID = "sa",
                .Password = "1122"
            }

            ' Logon Info to Main Report
            For Each table As Table In rptDoc.Database.Tables
                Dim logonInfo As TableLogOnInfo = table.LogOnInfo
                logonInfo.ConnectionInfo = connectionInfo
                table.ApplyLogOnInfo(logonInfo)
            Next

            ' Logon Info to Subreports
            For Each section As Section In rptDoc.ReportDefinition.Sections
                For Each reportObj As ReportObject In section.ReportObjects
                    If reportObj.Kind = ReportObjectKind.SubreportObject Then
                        Dim subreportObj As SubreportObject = CType(reportObj, SubreportObject)
                        Dim subReportDoc As ReportDocument = subreportObj.OpenSubreport(subreportObj.SubreportName)

                        For Each subTable As Table In subReportDoc.Database.Tables
                            Dim logonInfo As TableLogOnInfo = subTable.LogOnInfo
                            logonInfo.ConnectionInfo = connectionInfo
                            subTable.ApplyLogOnInfo(logonInfo)
                        Next
                    End If
                Next
            Next

            ' 3. Verify Database
            rptDoc.VerifyDatabase()

            ' 4. Convert Values for Parameters
            Dim fromYr As Integer = 0
            Dim toYr As Integer = 0
            Dim fromPrd As Integer = 0
            Dim toPrd As Integer = 0

            Integer.TryParse(ParamFromYear, fromYr)
            Integer.TryParse(ParamToYear, toYr)
            Integer.TryParse(ParamFromPeriod, fromPrd)
            Integer.TryParse(ParamToPeriod, toPrd)

            ' 5. Pass Parameters to Main Report Safely
            ApplyReportParameters(rptDoc, fromYr, toYr, fromPrd, toPrd)

            ' 6. Pass parameters to Subreports (Proper Subreport Iteration)
            For Each section As Section In rptDoc.ReportDefinition.Sections
                For Each reportObj As ReportObject In section.ReportObjects
                    If reportObj.Kind = ReportObjectKind.SubreportObject Then
                        Dim subreportObj As SubreportObject = CType(reportObj, SubreportObject)
                        Dim subReportDoc As ReportDocument = subreportObj.OpenSubreport(subreportObj.SubreportName)

                        ' Apply Parameters to Subreport
                        ApplyReportParameters(subReportDoc, fromYr, toYr, fromPrd, toPrd)
                    End If
                Next
            Next

            ' 7. Bind Report to Viewer
            CrystalReportViewer1.ReportSource = rptDoc
            CrystalReportViewer1.Refresh()

        Catch ex As Exception
            MessageBox.Show("Report load korar somoy somossha hoyeche: " & ex.Message, "Report Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Common Method to Pass All Single & Multi-Value Parameters
    Private Sub ApplyReportParameters(ByRef rpt As ReportDocument, ByVal fromYr As Integer, ByVal toYr As Integer, ByVal fromPrd As Integer, ByVal toPrd As Integer)
        SetSafeParameter(rpt, "FromAccID", If(ParamFromAccID, "").Trim())
        SetSafeParameter(rpt, "ToAccID", If(ParamToAccID, "").Trim())
        SetSafeParameter(rpt, "FromYear", fromYr)
        SetSafeParameter(rpt, "ToYear", toYr)
        SetSafeParameter(rpt, "FromPeriod", fromPrd)
        SetSafeParameter(rpt, "ToPeriod", toPrd)

        SetSafeParameter(rpt, "FromAccounts", If(ParamFromAccounts, "").Trim())
        SetSafeParameter(rpt, "ToAccounts", If(ParamToAccounts, "").Trim())
        SetSafeParameter(rpt, "FromEntity", If(ParamFromEntity, "").Trim())
        SetSafeParameter(rpt, "ToEntity", If(ParamToEntity, "").Trim())
        SetSafeParameter(rpt, "FromConcern", If(ParamFromConcern, "").Trim())
        SetSafeParameter(rpt, "ToConcern", If(ParamToConcern, "").Trim())
        SetSafeParameter(rpt, "FromFormattedAcc", If(ParamFromFormattedAcc, "").Trim())
        SetSafeParameter(rpt, "ToFormattedAcc", If(ParamToFormattedAcc, "").Trim())

        ' Display Text Parameter (Header Value Pass)
        SetSafeParameter(rpt, "SourceModulesDisplayParam", If(ParamSourceModulesDisplay, "").Trim())

        ' Source Modules Multi-Value Parameter Pass
        Dim sourceArr As String() = If(ParamSourceModulesList IsNot Nothing AndAlso ParamSourceModulesList.Count > 0, ParamSourceModulesList.ToArray(), New String() {})
        SetSafeMultiValueParameter(rpt, "SourceType", sourceArr)
    End Sub

    ' Helper Function: Single Parameter
    Private Sub SetSafeParameter(ByRef rpt As ReportDocument, ByVal paramName As String, ByVal val As Object)
        For Each p As ParameterFieldDefinition In rpt.DataDefinition.ParameterFields
            If p.ParameterFieldName.Equals(paramName, StringComparison.OrdinalIgnoreCase) AndAlso Not p.IsLinked Then
                rpt.SetParameterValue(paramName, val)
                Exit Sub
            End If
        Next
    End Sub

    ' Helper Function: Multi-Value Parameter
    Private Sub SetSafeMultiValueParameter(ByRef rpt As ReportDocument, ByVal paramName As String, ByVal values As String())
        For Each p As ParameterFieldDefinition In rpt.DataDefinition.ParameterFields
            If p.ParameterFieldName.Equals(paramName, StringComparison.OrdinalIgnoreCase) AndAlso Not p.IsLinked Then
                Dim paramValues As New ParameterValues()
                For Each item As String In values
                    Dim discreteVal As New ParameterDiscreteValue()
                    discreteVal.Value = item
                    paramValues.Add(discreteVal)
                Next
                rpt.DataDefinition.ParameterFields(paramName).ApplyCurrentValues(paramValues)
                Exit Sub
            End If
        Next
    End Sub

    Private Function FindReportFilePath(ByVal fileName As String) As String
        Dim specificPath As String = Path.Combine("C:\Users\ASUS\source\repos\General Ledger Report2\General Ledger Report2\Report", fileName)
        If File.Exists(specificPath) Then Return specificPath

        Dim currentDir As String = Application.StartupPath
        Dim directPath As String = Path.Combine(currentDir, fileName)
        If File.Exists(directPath) Then Return directPath

        Dim dirInfo As New DirectoryInfo(currentDir)
        Dim depth As Integer = 0

        While dirInfo IsNot Nothing AndAlso depth < 4
            Try
                Dim reportFolder As String = Path.Combine(dirInfo.FullName, "Report", fileName)
                If File.Exists(reportFolder) Then Return reportFolder

                Dim files() As String = Directory.GetFiles(dirInfo.FullName, fileName, SearchOption.TopDirectoryOnly)
                If files.Length > 0 Then Return files(0)
            Catch ex As Exception
            End Try

            dirInfo = dirInfo.Parent
            depth += 1
        End While

        Return String.Empty
    End Function
End Class
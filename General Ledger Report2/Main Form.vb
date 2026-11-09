Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Windows.Forms

Public Class Main_Form

    ' Connection String
    Private ReadOnly constr As String = "Server=LAPTOP-8VQAAFE9;Database=BNBDAT;User Id=sa;Password=1122;"

    ' Source Module tracking Variables
    Private ReadOnly allModules As New List(Of String)()
    Private ReadOnly selectedModules As New List(Of String)()

    ' Main Account Data Table
    Private ReadOnly dtAccounts As New DataTable()

    Private Sub Main_Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' ComboBox Search trigger events
        AddHandler FromAccID.DropDown, AddressOf OpenSearchForm
        AddHandler ToAccID.DropDown, AddressOf OpenSearchForm

        ' Set ComboBox DropDown Styles
        FromYaear.DropDownStyle = ComboBoxStyle.DropDownList
        ToYear.DropDownStyle = ComboBoxStyle.DropDownList
        FromPeriod.DropDownStyle = ComboBoxStyle.DropDownList
        ToPeriod.DropDownStyle = ComboBoxStyle.DropDownList

        ' Load Data Functions
        LoadAccountData()
        LoadYearAndPeriodData()
        SetupAndLoadSegmentGrid()
        LoadSourceModules()
    End Sub

    ' Open Search Form on ComboBox Click Event
    Private Sub OpenSearchForm(ByVal sender As Object, ByVal e As EventArgs)
        Dim cmb As ComboBox = DirectCast(sender, ComboBox)

        ' Close native dropdown list immediately
        BeginInvoke(New Action(Sub() cmb.DroppedDown = False))

        If dtAccounts IsNot Nothing AndAlso dtAccounts.Rows.Count > 0 Then
            Using searchFrm As New Search(dtAccounts)
                If searchFrm.ShowDialog(Me) = DialogResult.OK Then
                    If Not String.IsNullOrEmpty(searchFrm.SelectedAccount) Then
                        cmb.SelectedValue = searchFrm.SelectedAccount
                    End If
                End If
            End Using
        End If
    End Sub

    ' Load Accounts Data
    Private Sub LoadAccountData()
        Try
            Using con As New SqlConnection(constr)
                con.Open()

                Dim query As String = "SELECT DISTINCT " &
                                      "  [FORMATTED ACCOOUNTS], " &
                                      "  ACCTDESC, " &
                                      "  ISNULL(Status, '') AS Status " &
                                      "FROM VIEW_GL_TRANSACTION_LISTING " &
                                      "WHERE [FORMATTED ACCOOUNTS] IS NOT NULL AND [FORMATTED ACCOOUNTS] <> '' " &
                                      "ORDER BY [FORMATTED ACCOOUNTS]"

                Using da As New SqlDataAdapter(query, con)
                    dtAccounts.Clear()
                    da.Fill(dtAccounts)
                End Using

                ' FromAccID DataSource
                FromAccID.DataSource = dtAccounts.Copy()
                FromAccID.DisplayMember = "FORMATTED ACCOOUNTS"
                FromAccID.ValueMember = "FORMATTED ACCOOUNTS"

                ' ToAccID DataSource
                Dim dtTo As DataTable = dtAccounts.Copy()
                If dtTo.Select("[FORMATTED ACCOOUNTS] = 'ZZZ'").Length = 0 Then
                    Dim newRow As DataRow = dtTo.NewRow()
                    newRow("FORMATTED ACCOOUNTS") = "ZZZ"
                    newRow("ACCTDESC") = "ZZZ"
                    newRow("Status") = ""
                    dtTo.Rows.Add(newRow)
                End If

                ToAccID.DataSource = dtTo
                ToAccID.DisplayMember = "FORMATTED ACCOOUNTS"
                ToAccID.ValueMember = "FORMATTED ACCOOUNTS"

                ' By Default kono selection thakbe na (khali thakle Show Report-e first/last nibe)
                FromAccID.SelectedIndex = -1
                ToAccID.SelectedIndex = -1
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading accounts: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Load Year and Period Data
    Private Sub LoadYearAndPeriodData()
        Try
            Using con As New SqlConnection(constr)
                con.Open()

                ' Year Load
                Dim queryYr As String = "SELECT DISTINCT FISCALYR FROM VIEW_GL_TRANSACTION_LISTING WHERE FISCALYR IS NOT NULL ORDER BY FISCALYR"
                Dim dtYr1 As New DataTable()
                Using daYr As New SqlDataAdapter(queryYr, con)
                    daYr.Fill(dtYr1)
                End Using

                FromYaear.DataSource = dtYr1
                FromYaear.DisplayMember = "FISCALYR"
                FromYaear.ValueMember = "FISCALYR"

                ToYear.DataSource = dtYr1.Copy()
                ToYear.DisplayMember = "FISCALYR"
                ToYear.ValueMember = "FISCALYR"

                FromYaear.SelectedIndex = -1
                ToYear.SelectedIndex = -1

                ' Period Load
                Dim queryPrd As String = "SELECT DISTINCT " &
                                         "  FISCALPERD, " &
                                         "  RIGHT('0' + LTRIM(RTRIM(CAST(FISCALPERD AS VARCHAR))), 2) AS DISPLAY_PERD, " &
                                         "  CAST(FISCALPERD AS INT) AS SORT_PERD " &
                                         "FROM VIEW_GL_TRANSACTION_LISTING " &
                                         "WHERE FISCALPERD IS NOT NULL AND LTRIM(RTRIM(CAST(FISCALPERD AS VARCHAR))) <> '' " &
                                         "ORDER BY SORT_PERD"

                Dim dtPrd1 As New DataTable()
                Using daPrd As New SqlDataAdapter(queryPrd, con)
                    daPrd.Fill(dtPrd1)
                End Using

                FromPeriod.DataSource = dtPrd1
                FromPeriod.DisplayMember = "DISPLAY_PERD"
                FromPeriod.ValueMember = "FISCALPERD"

                ToPeriod.DataSource = dtPrd1.Copy()
                ToPeriod.DisplayMember = "DISPLAY_PERD"
                ToPeriod.ValueMember = "FISCALPERD"

                FromPeriod.SelectedIndex = -1
                ToPeriod.SelectedIndex = -1
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading Year/Period: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ---------------------------------------------------------------------
    ' Combo khali thakle: first (isFirst=True) ba last (isFirst=False) item-er value
    ' ---------------------------------------------------------------------
    Private Function GetComboValueOrDefault(ByVal cmb As ComboBox, ByVal isFirst As Boolean) As String
        ' User select korle seta-i nibe
        If cmb.SelectedValue IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(cmb.SelectedValue.ToString()) Then
            Return cmb.SelectedValue.ToString()
        End If

        ' Khali thakle first / last item
        If cmb.Items.Count > 0 Then
            Dim idx As Integer = If(isFirst, 0, cmb.Items.Count - 1)
            Dim drv As DataRowView = TryCast(cmb.Items(idx), DataRowView)
            If drv IsNot Nothing Then
                Dim v As Object = drv(cmb.ValueMember)
                If v IsNot Nothing AndAlso v IsNot DBNull.Value Then Return v.ToString()
            End If
        End If

        Return String.Empty
    End Function

    ' Setup Segment Grid Data
    Private Sub SetupAndLoadSegmentGrid()
        Try
            DataGridView1.Columns.Clear()
            DataGridView1.Rows.Clear()

            DataGridView1.DefaultCellStyle.Font = New Font("Microsoft Sans Serif", 9.0!, FontStyle.Bold)
            DataGridView1.ColumnHeadersDefaultCellStyle.Font = New Font("Microsoft Sans Serif", 9.0!, FontStyle.Bold)
            DataGridView1.RowHeadersVisible = False
            DataGridView1.AllowUserToResizeColumns = False
            DataGridView1.AllowUserToResizeRows = False
            DataGridView1.AllowUserToAddRows = False
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            Dim colName As New DataGridViewTextBoxColumn With {
                .Name = "SegmentName",
                .HeaderText = "Segment Name",
                .ReadOnly = True,
                .FillWeight = 30
            }
            DataGridView1.Columns.Add(colName)

            Dim colFrom As New DataGridViewComboBoxColumn With {
                .Name = "FromVal",
                .HeaderText = "From",
                .DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing,
                .FlatStyle = FlatStyle.Standard,
                .FillWeight = 35
            }
            DataGridView1.Columns.Add(colFrom)

            Dim colTo As New DataGridViewComboBoxColumn With {
                .Name = "ToVal",
                .HeaderText = "To",
                .DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing,
                .FlatStyle = FlatStyle.Standard,
                .FillWeight = 35
            }
            DataGridView1.Columns.Add(colTo)

            Using con As New SqlConnection(constr)
                con.Open()

                Dim LoadSegmentRow = Sub(segmentName As String, columnName As String)
                                         Dim list As New List(Of String)()
                                         Dim qry As String = $"SELECT DISTINCT [{columnName}] FROM VIEW_GL_TRANSACTION_LISTING WHERE [{columnName}] IS NOT NULL AND [{columnName}] <> '' ORDER BY [{columnName}]"

                                         Using cmd As New SqlCommand(qry, con)
                                             Using reader As SqlDataReader = cmd.ExecuteReader()
                                                 While reader.Read()
                                                     list.Add(reader(columnName).ToString())
                                                 End While
                                             End Using
                                         End Using

                                         Dim rowIndex As Integer = DataGridView1.Rows.Add(segmentName)
                                         Dim cellFrom As DataGridViewComboBoxCell = CType(DataGridView1.Rows(rowIndex).Cells("FromVal"), DataGridViewComboBoxCell)
                                         Dim cellTo As DataGridViewComboBoxCell = CType(DataGridView1.Rows(rowIndex).Cells("ToVal"), DataGridViewComboBoxCell)

                                         For Each val As String In list
                                             cellFrom.Items.Add(val)
                                             cellTo.Items.Add(val)
                                         Next

                                         cellFrom.Value = Nothing
                                         cellTo.Value = Nothing
                                     End Sub

                ' Load Segments (Accounts, Entity, Concern)
                LoadSegmentRow("Accounts", "Accounts")
                LoadSegmentRow("Entity", "Entity")
                LoadSegmentRow("Concern", "Concern")
            End Using
        Catch ex As Exception
            MessageBox.Show("Segment grid loading error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Grid Cell Click event handling
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return

        Dim segName As String = If(DataGridView1.Rows(e.RowIndex).Cells("SegmentName").Value IsNot Nothing, DataGridView1.Rows(e.RowIndex).Cells("SegmentName").Value.ToString(), "")
        Dim colName As String = DataGridView1.Columns(e.ColumnIndex).Name

        If segName = "Entity" OrElse segName = "Concern" Then
            Return
        End If

        If colName = "FromVal" OrElse colName = "ToVal" Then
            Dim targetCell As DataGridViewComboBoxCell = CType(DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex), DataGridViewComboBoxCell)

            Dim columnNameInDB As String = ""
            If segName = "Accounts" Then
                columnNameInDB = "Accounts"
            End If

            If String.IsNullOrEmpty(columnNameInDB) Then Return

            Dim dtSeg As New DataTable()

            Try
                Using con As New SqlConnection(constr)
                    con.Open()
                    Dim query As String = String.Format("SELECT " &
                                                        "  ISNULL(ACCTID, '') AS ACCTID, " &
                                                        "  {0} AS AccountValue, " &
                                                        "  MAX(ISNULL(ACCTDESC, '')) AS Description, " &
                                                        "  MAX(ISNULL(Status, '')) AS Status " &
                                                        "FROM VIEW_GL_TRANSACTION_LISTING " &
                                                        "WHERE {0} IS NOT NULL AND {0} <> '' " &
                                                        "GROUP BY ACCTID, {0} " &
                                                        "ORDER BY {0}", columnNameInDB)

                    Using da As New SqlDataAdapter(query, con)
                        da.Fill(dtSeg)
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Error loading segment details: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End Try

            Using searchFrm As New SearchSegment(dtSeg, segName)
                If searchFrm.ShowDialog(Me) = DialogResult.OK Then
                    If Not String.IsNullOrWhiteSpace(searchFrm.SelectedValue) Then
                        If Not targetCell.Items.Contains(searchFrm.SelectedValue) Then
                            targetCell.Items.Add(searchFrm.SelectedValue)
                        End If

                        targetCell.Value = searchFrm.SelectedValue
                        DataGridView1.EndEdit()
                    End If
                End If
            End Using
        End If
    End Sub

    ' Load Source Modules (default-e SOB module selected thakbe)
    Private Sub LoadSourceModules()
        Try
            Using conn As New SqlConnection(constr)
                Dim query As String = "SELECT DISTINCT SRCELEDGER FROM VIEW_GL_TRANSACTION_LISTING WHERE SRCELEDGER IS NOT NULL AND SRCELEDGER <> '' ORDER BY SRCELEDGER"

                Using da As New SqlDataAdapter(query, conn)
                    Using dt As New DataTable()
                        da.Fill(dt)

                        allModules.Clear()
                        selectedModules.Clear()

                        For Each row As DataRow In dt.Rows
                            Dim modName As String = row("SRCELEDGER").ToString().Trim()
                            allModules.Add(modName)
                            ' Default: shob module selected
                            selectedModules.Add(modName)
                        Next
                    End Using
                End Using

                UpdateModuleTextBox()
            End Using
        Catch ex As Exception
            MessageBox.Show("Source Module Load Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub UpdateModuleTextBox()
        If selectedModules.Count = 0 Then
            txtSelectedModules.Text = ""
        Else
            txtSelectedModules.Text = String.Join(", ", selectedModules)
        End If
    End Sub

    ' Select Source Modules Modal Dialog
    Private Sub btnSelectModules_Click(sender As Object, e As EventArgs) Handles btnSelectModules.Click
        Using dlg As New Form()
            dlg.Text = "Select Source Modules"
            dlg.Size = New Size(340, 430)
            dlg.StartPosition = FormStartPosition.CenterParent
            dlg.FormBorderStyle = FormBorderStyle.FixedDialog
            dlg.MaximizeBox = False
            dlg.MinimizeBox = False

            Dim chkList As New CheckedListBox With {
                .Location = New Point(12, 12),
                .Size = New Size(300, 290),
                .CheckOnClick = True
            }

            For Each modName As String In allModules
                Dim isChecked As Boolean = selectedModules.Contains(modName)
                chkList.Items.Add(modName, isChecked)
            Next

            Dim btnSelectAll As New Button With {
                .Text = "Select All",
                .Size = New Size(90, 28),
                .Location = New Point(12, 312)
            }
            AddHandler btnSelectAll.Click, Sub(s, ev)
                                               For i As Integer = 0 To chkList.Items.Count - 1
                                                   chkList.SetItemChecked(i, True)
                                               Next
                                           End Sub

            Dim btnUnselectAll As New Button With {
                .Text = "Unselect All",
                .Size = New Size(90, 28),
                .Location = New Point(108, 312)
            }
            AddHandler btnUnselectAll.Click, Sub(s, ev)
                                                 For i As Integer = 0 To chkList.Items.Count - 1
                                                     chkList.SetItemChecked(i, False)
                                                 Next
                                             End Sub

            Dim btnOK As New Button With {
                .Text = "Apply",
                .DialogResult = DialogResult.OK,
                .Size = New Size(90, 28),
                .Location = New Point(222, 312)
            }

            dlg.Controls.Add(chkList)
            dlg.Controls.Add(btnSelectAll)
            dlg.Controls.Add(btnUnselectAll)
            dlg.Controls.Add(btnOK)
            dlg.AcceptButton = btnOK

            If dlg.ShowDialog(Me) = DialogResult.OK Then
                selectedModules.Clear()
                For Each item In chkList.CheckedItems
                    selectedModules.Add(item.ToString())
                Next

                UpdateModuleTextBox()
            End If
        End Using
    End Sub

    ' Show Report Button Handler
    Private Sub shwbutton_Click(sender As Object, e As EventArgs) Handles shwbutton.Click

        ' 1. Year / Period / Account: khali thakle first (From) ba last (To) item nibe
        Dim selFromYear As String = GetComboValueOrDefault(FromYaear, True)
        Dim selToYear As String = GetComboValueOrDefault(ToYear, False)
        Dim selFromPeriod As String = GetComboValueOrDefault(FromPeriod, True)
        Dim selToPeriod As String = GetComboValueOrDefault(ToPeriod, False)
        Dim selectedFromAcc As String = GetComboValueOrDefault(FromAccID, True)
        Dim selectedToAcc As String = GetComboValueOrDefault(ToAccID, False)

        ' Database-e kono data na thakle
        If selFromYear = "" OrElse selToYear = "" Then
            MessageBox.Show("No Fiscal Year data found.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If selFromPeriod = "" OrElse selToPeriod = "" Then
            MessageBox.Show("No Fiscal Period data found.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' 2. Source Module: kichu select na thakle error
        If selectedModules.Count = 0 Then
            MessageBox.Show("Please select at least one Source Module.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' 3. Open Report
        Using rptForm As New ReportViewer()
            rptForm.StartPosition = FormStartPosition.Manual
            rptForm.Location = New Point(Me.Left + 100, Me.Top)

            rptForm.ParamFromAccID = selectedFromAcc
            rptForm.ParamToAccID = selectedToAcc
            rptForm.ParamFromYear = selFromYear
            rptForm.ParamToYear = selToYear
            rptForm.ParamFromPeriod = selFromPeriod
            rptForm.ParamToPeriod = selToPeriod

            ' Raw list for query filter + joined text for header display
            rptForm.ParamSourceModulesList = New List(Of String)(selectedModules)
            rptForm.ParamSourceModulesDisplay = String.Join(", ", selectedModules)

            ' Grid Segments parameters
            For Each row As DataGridViewRow In DataGridView1.Rows
                If row.Cells("SegmentName").Value IsNot Nothing Then
                    Dim segName As String = row.Cells("SegmentName").Value.ToString()
                    Dim fromVal As String = If(row.Cells("FromVal").Value IsNot Nothing, row.Cells("FromVal").Value.ToString(), "")
                    Dim toVal As String = If(row.Cells("ToVal").Value IsNot Nothing, row.Cells("ToVal").Value.ToString(), "")

                    Select Case segName
                        Case "Accounts"
                            rptForm.ParamFromAccounts = fromVal
                            rptForm.ParamToAccounts = toVal
                        Case "Entity"
                            rptForm.ParamFromEntity = fromVal
                            rptForm.ParamToEntity = toVal
                        Case "Concern"
                            rptForm.ParamFromConcern = fromVal
                            rptForm.ParamToConcern = toVal
                    End Select
                End If
            Next

            rptForm.ShowDialog()
        End Using
    End Sub

    ' Refresh Button Handler
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            ' Reload all dropdown data from database
            ' (LoadSourceModules shob module abar default-e select kore dey)
            LoadAccountData()
            LoadYearAndPeriodData()
            SetupAndLoadSegmentGrid()
            LoadSourceModules()

            MessageBox.Show("All data refreshed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error refreshing data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class
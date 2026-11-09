Imports System.Data
Imports System.Windows.Forms

Public Class SearchSegment

    Public Property SelectedValue As String = ""
    Private dtSource As DataTable
    Private dvSource As DataView
    Private segmentName As String

    ' Constructor: SQL Query থেকে ৪টি কলাম (ACCTID, AccountValue, Description, Status) সহ DataTable রিসিভ করবে
    Public Sub New(ByVal dataSource As DataTable, ByVal segName As String)
        InitializeComponent()
        If dataSource IsNot Nothing Then
            Me.dtSource = dataSource.Copy()
            Me.dvSource = Me.dtSource.DefaultView
        End If
        Me.segmentName = segName
    End Sub

    Private Sub SearchSegment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Search - " & segmentName
        Me.StartPosition = FormStartPosition.CenterParent
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False

        ' Populate Dropdown (Category - Column Selection, ACCTID সরানো হয়েছে)
        Catagory.DropDownStyle = ComboBoxStyle.DropDownList
        Catagory.Items.Clear()
        Catagory.Items.Add("All")
        Catagory.Items.Add(If(String.IsNullOrWhiteSpace(segmentName), "Accounts", segmentName))
        Catagory.Items.Add("Description")
        Catagory.Items.Add("Status")
        Catagory.SelectedIndex = 0

        ' Populate Dropdown (Category2 - Search Mode)
        Catagory2.DropDownStyle = ComboBoxStyle.DropDownList
        Catagory2.Items.Clear()
        Catagory2.Items.Add("Starts with")
        Catagory2.Items.Add("Contains")
        Catagory2.SelectedIndex = 0

        ' DataGridView Setup
        If dvSource IsNot Nothing Then
            dgvSearch.DataSource = Nothing
            dgvSearch.Columns.Clear()
            dgvSearch.AutoGenerateColumns = True

            dgvSearch.DataSource = dvSource
            dgvSearch.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgvSearch.MultiSelect = False
            dgvSearch.ReadOnly = True
            dgvSearch.AllowUserToAddRows = False
            dgvSearch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            dgvSearch.RowHeadersVisible = False

            ' ১. ACCTID Column Hide
            If dgvSearch.Columns.Contains("ACCTID") Then
                dgvSearch.Columns("ACCTID").Visible = False
            End If

            ' ২. Percentages / FillWeight Setup
            ' Account Value (20%)
            If dgvSearch.Columns.Contains("AccountValue") Then
                dgvSearch.Columns("AccountValue").HeaderText = If(String.IsNullOrWhiteSpace(segmentName), "Accounts", segmentName)
                dgvSearch.Columns("AccountValue").FillWeight = 20
            End If

            ' Description (65%)
            If dgvSearch.Columns.Contains("Description") Then
                dgvSearch.Columns("Description").HeaderText = "Description"
                dgvSearch.Columns("Description").FillWeight = 65
            End If

            ' Status (15%)
            If dgvSearch.Columns.Contains("Status") Then
                dgvSearch.Columns("Status").HeaderText = "Status"
                dgvSearch.Columns("Status").FillWeight = 15
            End If
        End If

        txtSearch.Focus()
    End Sub

    ' Real-time Dynamic Filtering Logic
    Private Sub ApplySearchFilter()
        If dvSource Is Nothing Then Return

        Dim filterText As String = txtSearch.Text.Trim().Replace("'", "''")
        Dim selectedCategory As String = If(Catagory.SelectedItem IsNot Nothing, Catagory.SelectedItem.ToString(), "All")
        Dim searchMode As String = If(Catagory2.SelectedItem IsNot Nothing, Catagory2.SelectedItem.ToString(), "Starts with")

        If String.IsNullOrEmpty(filterText) Then
            dvSource.RowFilter = ""
        Else
            Dim searchVal As String
            If searchMode = "Contains" Then
                searchVal = String.Format("%{0}%", filterText)
            Else
                searchVal = String.Format("{0}%", filterText)
            End If

            If selectedCategory = segmentName OrElse selectedCategory = "Accounts" Then
                dvSource.RowFilter = String.Format("[AccountValue] LIKE '{0}'", searchVal)
            Else
                Select Case selectedCategory
                    Case "Description"
                        dvSource.RowFilter = String.Format("[Description] LIKE '{0}'", searchVal)

                    Case "Status"
                        dvSource.RowFilter = String.Format("[Status] LIKE '{0}'", searchVal)

                    Case Else ' "All" Option (ACCTID ব্যাকগ্রাউন্ডে সার্চ রাখবে)
                        dvSource.RowFilter = String.Format("[ACCTID] LIKE '{0}' OR [AccountValue] LIKE '{0}' OR [Description] LIKE '{0}' OR [Status] LIKE '{0}'", searchVal)
                End Select
            End If
        End If
    End Sub

    ' Event Handlers for Filtering
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        ApplySearchFilter()
    End Sub

    Private Sub Catagory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Catagory.SelectedIndexChanged
        ApplySearchFilter()
    End Sub

    Private Sub Catagory2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Catagory2.SelectedIndexChanged
        ApplySearchFilter()
    End Sub

    ' Key & Click Selection Events
    Private Sub dgvSearch_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvSearch.CellDoubleClick
        If e.RowIndex >= 0 Then
            SelectAndClose()
        End If
    End Sub

    Private Sub txtSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            SelectAndClose()
            e.Handled = True
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub dgvSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            SelectAndClose()
            e.Handled = True
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub btnSelect_Click(sender As Object, e As EventArgs) Handles btnSelect.Click
        ApplySearchFilter()
        SelectAndClose()
    End Sub

    ' FIXED: Cancel Button Handler corrected
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnSelect.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SelectAndClose()
        If dgvSearch.CurrentRow IsNot Nothing Then
            If dgvSearch.CurrentRow.Cells("AccountValue").Value IsNot DBNull.Value Then
                SelectedValue = dgvSearch.CurrentRow.Cells("AccountValue").Value.ToString()
            End If
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub

End Class
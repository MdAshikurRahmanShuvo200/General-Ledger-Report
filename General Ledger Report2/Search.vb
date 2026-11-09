Imports System.Data
Imports System.Windows.Forms

Public Class Search
    Public Property SelectedAccount As String = ""
    Private dtSource As DataTable
    Private dvSource As DataView

    ' Constructor to receive data from Main_Form
    Public Sub New(ByVal dt As DataTable)
        InitializeComponent()
        If dt IsNot Nothing Then
            Me.dtSource = dt.Copy()
            Me.dvSource = Me.dtSource.DefaultView
        End If
    End Sub

    Private Sub Search_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Search Account"
        Me.StartPosition = FormStartPosition.CenterParent

        ' Dropdown (Catagory - Column Selection) Setup
        Catagory.DropDownStyle = ComboBoxStyle.DropDownList
        Catagory.Items.Clear()
        Catagory.Items.Add("All")
        Catagory.Items.Add("FORMATTED ACCOOUNTS")
        Catagory.Items.Add("ACCTDESC")
        Catagory.Items.Add("Status")
        Catagory.SelectedIndex = 0 ' Default "All" selected thakbe

        ' Dropdown (Catagory2 - Search Mode) Setup
        Catagory2.DropDownStyle = ComboBoxStyle.DropDownList
        Catagory2.Items.Clear()
        Catagory2.Items.Add("Starts with")
        Catagory2.Items.Add("Contains")
        Catagory2.SelectedIndex = 0 ' Default "Starts with" selected thakbe

        If dvSource IsNot Nothing Then
            ' Clear old designer columns if any
            dgvResults.DataSource = Nothing
            dgvResults.Columns.Clear()
            dgvResults.AutoGenerateColumns = True

            dgvResults.DataSource = dvSource
            dgvResults.ReadOnly = True
            dgvResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgvResults.MultiSelect = False
            dgvResults.AllowUserToAddRows = False
            dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            ' ---- REMOVE FIRST BLANK COLUMN / ROW HEADER ----
            dgvResults.RowHeadersVisible = False

            ' ---- HEADER RENAME & WIDTH ADJUSTMENT ----
            If dgvResults.Columns.Contains("FORMATTED ACCOOUNTS") Then
                dgvResults.Columns("FORMATTED ACCOOUNTS").HeaderText = "Accounts ID"
                dgvResults.Columns("FORMATTED ACCOOUNTS").FillWeight = 25
            End If

            If dgvResults.Columns.Contains("ACCTDESC") Then
                dgvResults.Columns("ACCTDESC").HeaderText = "Description"
                dgvResults.Columns("ACCTDESC").FillWeight = 60
            End If

            If dgvResults.Columns.Contains("Status") Then
                dgvResults.Columns("Status").HeaderText = "Status"
                dgvResults.Columns("Status").FillWeight = 15
            End If
        End If
    End Sub

    ' Search Logic Function
    Private Sub ApplySearchFilter()
        If dvSource Is Nothing Then Return

        Dim filterText As String = txtSearch.Text.Trim().Replace("'", "''")
        Dim selectedCategory As String = If(Catagory.SelectedItem IsNot Nothing, Catagory.SelectedItem.ToString(), "All")
        Dim searchMode As String = If(Catagory2.SelectedItem IsNot Nothing, Catagory2.SelectedItem.ToString(), "Starts with")

        If String.IsNullOrEmpty(filterText) Then
            dvSource.RowFilter = ""
        Else
            ' Search Pattern Definition
            Dim searchVal As String
            If searchMode = "Contains" Then
                searchVal = String.Format("%{0}%", filterText) ' Text-er jekono jaigay match korle hobe
            Else
                searchVal = String.Format("{0}%", filterText)  ' Starts with: Text-er prothom thake match korbe
            End If

            Select Case selectedCategory
                Case "FORMATTED ACCOOUNTS"
                    dvSource.RowFilter = String.Format("[FORMATTED ACCOOUNTS] LIKE '{0}'", searchVal)

                Case "ACCTDESC"
                    dvSource.RowFilter = String.Format("ACCTDESC LIKE '{0}'", searchVal)

                Case "Status"
                    dvSource.RowFilter = String.Format("Status LIKE '{0}'", searchVal)

                Case Else ' "All" Option
                    dvSource.RowFilter = String.Format("[FORMATTED ACCOOUNTS] LIKE '{0}' OR ACCTDESC LIKE '{0}' OR Status LIKE '{0}'", searchVal)
            End Select
        End If
    End Sub

    ' Real-time live filtering on typing
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        ApplySearchFilter()
    End Sub

    ' Column Category Change hoile auto refilter
    Private Sub Catagory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Catagory.SelectedIndexChanged
        ApplySearchFilter()
    End Sub

    ' Search Mode (Starts with / Contains) Change hoile auto refilter
    Private Sub Catagory2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Catagory2.SelectedIndexChanged
        ApplySearchFilter()
    End Sub

    Private Sub dgvResults_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvResults.CellDoubleClick
        If e.RowIndex >= 0 Then
            ConfirmSelection()
        End If
    End Sub

    ' Button Click Event (Search filter re-verify & Select Row)
    Private Sub btnSelect_Click(sender As Object, e As EventArgs) Handles btnSelect.Click
        ApplySearchFilter()
        ConfirmSelection()
    End Sub

    Private Sub ConfirmSelection()
        If dgvResults.CurrentRow IsNot Nothing Then
            SelectedAccount = dgvResults.CurrentRow.Cells("FORMATTED ACCOOUNTS").Value.ToString()
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub
End Class
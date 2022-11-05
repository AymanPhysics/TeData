Imports System.Data

Public Class Invoices5
    Dim MainTableName As String = "Branches"
    Dim MainSubId As String = "Id"
    Dim MainSubName As String = "Name"


    Dim Main2TableName As String = "Employees"
    Dim Main2MainId As String = "BranchId"
    Dim Main2SubId As String = "Id"
    Dim Main2SubName As String = "ArName"


    Public TableName As String = "Invoices50"
    Dim MainId As String = "BranchId"
    Dim MainId2 As String = "EmpId"
    Dim SubId As String = "InvoiceNo"
    Dim SubName As String = "Name"

    Public lblMain_Content As String
    Public lblMain2_Content As String


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0

    Private Sub BasicForm3_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        'LoadResource()
        bm.Fields = New String() {MainId, MainId2, SubId, "DayDate", "IncomeTypeId", "Qty", "Notes"}
        bm.control = New Control() {CboMain, CboMain2, txtID, DayDate, IncomeTypeId, Qty, Notes}

        bm.KeyFields = New String() {MainId, MainId2, SubId}

        CboMain.IsEnabled = Md.Manager
        CboMain2.IsEnabled = Md.Manager

        bm.FillCombo(MainTableName, CboMain, "")
        bm.FillCombo("select 0 Id,'-' Name union select cast(Id as varchar(100)) Id,Name from IncomeTypes where HasBalance=1", IncomeTypeId)

        bm.Table_Name = TableName
        btnNew_Click(sender, e)
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {MainId, MainId2, SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {MainId, MainId2, SubId}, New String() {CboMain.SelectedValue.ToString, CboMain2.SelectedValue.ToString, txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If CboMain.SelectedValue.ToString = 0 OrElse CboMain2.SelectedValue.ToString = 0 OrElse Val(Qty.Text.Trim) = 0 Then
            Return
        End If
        DelivaryCost.Text = Val(DelivaryCost.Text)
        Qty.Text = Val(Qty.Text)
        bm.DefineValues()
        If Not bm.Save(New String() {MainId, MainId2, SubId}, New String() {CboMain.SelectedValue.ToString, CboMain2.SelectedValue.ToString, txtID.Text.Trim}) Then Return
        btnNew_Click(sender, e)
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click

        bm.FirstLast(New String() {MainId, MainId2, SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        'bm.ClearControls()
        Try
            CboMain.SelectedValue = Md.BranchId
            CboMain_SelectedIndexChanged(Nothing, Nothing)
            CboMain2.SelectedValue = Md.UserName
            CboMain2_SelectedIndexChanged(Nothing, Nothing)
        Catch
        End Try
    End Sub

    Sub ClearControls()
        lop = True
        Try
            bm.ClearControls(False)
            DayDate.SelectedDate = bm.MyGetDate()
            DelivaryCost.Clear()
            Qty.Text = 1
            txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName & " where " & MainId & "='" & CboMain.SelectedValue.ToString & "' and " & MainId2 & "='" & CboMain2.SelectedValue.ToString & "'")
            If txtID.Text = "" Then txtID.Text = "1"

            DayDate.Focus()
        Catch
        End Try
        lop = False
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExcuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "' and " & MainId & " ='" & CboMain.SelectedValue.ToString & "' and " & MainId2 & " ='" & CboMain2.SelectedValue.ToString & "'")
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {MainId, MainId2, SubId}, New String() {CboMain.SelectedValue.ToString, CboMain2.SelectedValue.ToString, txtID.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False
    Private Sub txtID_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {MainId, MainId2, SubId}, New String() {CboMain.SelectedValue.ToString, CboMain2.SelectedValue.ToString, txtID.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub DelivaryCost_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Qty.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub

    Private Sub CboMain_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboMain.SelectionChanged
        If lop Then Return
        Dim s As String = ""
        Try
            s = CboMain.SelectedValue.ToString
        Catch ex As Exception
        End Try
        bm.FillCombo("select 0 Id, '-' Name union Select Id,ArName Name From Employees where BranchId='" & s & "' order by Name", CboMain2)
        ClearControls()
    End Sub

    Private Sub CboMain2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboMain2.SelectionChanged
        If lop Then Return
        ClearControls()
    End Sub

    Dim lop As Boolean = False
    Private Sub FillControls()
        lop = True
        bm.FillControls()
        lop = False
    End Sub

End Class

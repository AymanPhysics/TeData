Imports System.Data

Public Class Invoices4
    Dim MainTableName As String = "Branches"
    Dim MainSubId As String = "Id"
    Dim MainSubName As String = "Name"


    Dim Main2TableName As String = "Employees"
    Dim Main2MainId As String = "BranchId"
    Dim Main2SubId As String = "Id"
    Dim Main2SubName As String = "ArName"


    Dim TableName As String = "Invoices4"
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
        bm.Fields = New String() {MainId, MainId2, SubId, "DayDate", "CustId", "ProviderId", "ProviderRateId", "ProviderPromotionId", "OrderTypeId", "OrderDate", "Done", "DoneDate", "Notes", "TicketNo"}
        bm.control = New Control() {CboMain, CboMain2, txtID, DayDate, CustId, ProviderId, ProviderRateId, ProviderPromotionId, OrderTypeId, OrderDate, Done, DoneDate, Notes, TicketNo}

        bm.KeyFields = New String() {MainId, MainId2, SubId}

        CboMain.IsEnabled = Md.Manager
        CboMain2.IsEnabled = Md.Manager
        
        ProviderId.IsEnabled = False
        ProviderRateId.IsEnabled = False
        ProviderPromotionId.IsEnabled = False

        bm.FillCombo(MainTableName, CboMain, "")
        bm.FillCombo("OrderTypes", OrderTypeId, "")
        bm.FillCombo("Providers", ProviderId, "", , True)
        bm.FillCombo("CustomerStatuses", CustomerStatusId, "", , True)

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
        If CboMain.SelectedValue.ToString = 0 OrElse CboMain2.SelectedValue.ToString = 0 Then
            Return
        End If
        
        bm.DefineValues()
        If Not bm.Save(New String() {MainId, MainId2, SubId}, New String() {CboMain.SelectedValue.ToString, CboMain2.SelectedValue.ToString, txtID.Text.Trim}) Then Return
        'bm.ExcuteNonQuery("update Customers set CustomerStatusId='" & CustomerStatusId.SelectedValue.ToString & "',Notes='" & Notes.Text.Trim & "' where Id=" & CustId.Text.Trim())
        bm.ExcuteNonQuery("ChangeCustomerStatus", {"EmpId", "CustId", "CustomerStatusId", "Notes"}, {Md.UserName, CustId.Text.Trim(), CustomerStatusId.SelectedValue.ToString, Notes.Text.Trim})
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
            Done_Unchecked(Nothing, Nothing)
            CustomerStatusId.SelectedIndex = 0
            DayDate.SelectedDate = bm.MyGetDate()
            OrderDate.SelectedDate = bm.MyGetDate()
            DelivaryCost.Clear()
            CustName.Clear()
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

    Private Sub DelivaryCost_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs)
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
        CustId_LostFocus(Nothing, Nothing)
        lop = False
    End Sub

    Private Sub CustId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles CustId.KeyUp
        If bm.ShowHelp("Customers", CustId, CustName, e, "select cast(Id as varchar(100)) Id,Name from Customers") Then
            CustId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub ProviderId_LostFocus(sender As Object, e As RoutedEventArgs) Handles ProviderId.LostFocus
        If ProviderId.SelectedIndex < 0 Then Return
        bm.FillCombo("ProviderRates", ProviderRateId, "where ProviderId='" & ProviderId.SelectedValue.ToString & "'", , True)
        bm.FillCombo("ProviderPromotions", ProviderPromotionId, "where ProviderId='" & ProviderId.SelectedValue.ToString & "'", , True)
    End Sub

    Private Sub CustId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CustId.LostFocus
        bm.LostFocus(CustId, CustName, "select Name from Customers where Id=" & CustId.Text.Trim())
        bm.LostFocus(CustId, Notes, "select Notes from Customers where Id=" & CustId.Text.Trim())
        ProviderId.SelectedValue = Val(bm.ExecuteScalar("select ProviderId from Customers where Id=" & CustId.Text.Trim()))
        ProviderId_LostFocus(Nothing, Nothing)
        ProviderRateId.SelectedValue = Val(bm.ExecuteScalar("select ProviderRateId from Customers where Id=" & CustId.Text.Trim()))
        ProviderPromotionId.SelectedValue = Val(bm.ExecuteScalar("select ProviderPromotionId from Customers where Id=" & CustId.Text.Trim()))
        CustomerStatusId.SelectedValue = Val(bm.ExecuteScalar("select CustomerStatusId from Customers where Id=" & CustId.Text.Trim()))
    End Sub

    Private Sub CustomerStatusId_LostFocus(sender As Object, e As RoutedEventArgs) Handles CustomerStatusId.LostFocus
        If Not Md.Manager Then
            If Val(bm.ExecuteScalar("select Stopped from CustomerStatuses where Id=" & CustomerStatusId.SelectedValue)) Then
                bm.ShowMSG("برجاء اختيار حالة أخرى أو العودة لمدير النظام")
                CustomerStatusId.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub Done_Checked(sender As Object, e As RoutedEventArgs) Handles Done.Checked
        DoneDate.Visibility = Windows.Visibility.Visible
        DoneDate.SelectedDate = bm.MyGetDate()
    End Sub

    Private Sub Done_Unchecked(sender As Object, e As RoutedEventArgs) Handles Done.Unchecked
        DoneDate.Visibility = Windows.Visibility.Hidden
    End Sub

End Class

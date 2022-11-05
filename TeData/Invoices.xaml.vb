Imports System.Data

Public Class Invoices
    Dim MainTableName As String = "Branches"
    Dim MainSubId As String = "Id"
    Dim MainSubName As String = "Name"


    Dim Main2TableName As String = "Employees"
    Dim Main2MainId As String = "BranchId"
    Dim Main2SubId As String = "Id"
    Dim Main2SubName As String = "ArName"


    Dim TableName As String = "Invoices"
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
        bm.Fields = New String() {MainId, MainId2, SubId, "IsPrev", "DayDate", "CustId", "ProviderId", "ProviderRateId", "ProviderPromotionId", "IncomeTypeId", "ActivateDate", "RechargeDate", "Qty", "Price", "Value", "Notes", "Payed", "Remaining", "Bal0", "Time", "Done"}
        bm.control = New Control() {CboMain, CboMain2, txtID, IsPrev, DayDate, CustId, ProviderId, ProviderRateId, ProviderPromotionId, IncomeTypeId, ActivateDate, RechargeDate, Qty, Price, Value, Notes, Payed, Remaining, Bal0, Time, Done}
    
        bm.KeyFields = New String() {MainId, MainId2, SubId}

        CboMain.IsEnabled = Md.Manager
        CboMain2.IsEnabled = Md.Manager
        txtID.IsEnabled = Md.Manager
        ActivateDate.IsEnabled = Md.Manager

        If Not Md.Manager Then
            btnFirst.Visibility = Windows.Visibility.Hidden
            btnPrevios.Visibility = Windows.Visibility.Hidden
            btnNext.Visibility = Windows.Visibility.Hidden
            btnLast.Visibility = Windows.Visibility.Hidden
            btnDelete.Visibility = Windows.Visibility.Hidden
        End If


        ProviderId.IsEnabled = False
        ProviderRateId.IsEnabled = False
        ProviderPromotionId.IsEnabled = False

        bm.FillCombo(MainTableName, CboMain, "")
        bm.FillCombo(MainTableName, CustBranchId, "")
        bm.FillCombo("select * from(select -1 Id,'** تجديد الاشتراك **' Name union select -2 Id,'** تجديد اشتراك خارجي**' Name union select 0 Id,'-' Name union select cast(Id as varchar(100)) Id,Name from IncomeTypes)tbl order by (case when Id<=0 then -1 else 0 end),abs(Id),Id", IncomeTypeId)
        bm.FillCombo("Providers", ProviderId, "", , True)
        bm.FillCombo("CustomerStatuses", CustomerStatusId, "", , True)
       
        bm.Table_Name = TableName
        btnNew_Click(sender, e)
    End Sub

    Sub NewId()
        txtID.Clear()
        txtID.IsEnabled = False
    End Sub

    Sub UndoNewId()
        txtID.IsEnabled = True
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

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnPrint.Click
        If CboMain.SelectedValue.ToString = 0 OrElse CboMain2.SelectedValue.ToString = 0 Then
            Return
        End If


        'If Val(Value.Text) < 0 Then Return
        'If Val(Payed.Text) < 0 Then Return

        If IncomeTypeId.SelectedValue = -1 AndAlso Val(Price.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد السعر")
            Return
        End If

        If IncomeTypeId.SelectedValue = -1 AndAlso Val(Payed.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد المدفوع")
            Return
        End If

        If Val(Qty.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد العدد")
            Return
        End If

        If IncomeTypeId.SelectedValue < 0 AndAlso bm.IF_Exists("select CustId from Invoices where CustId='" & CustId.Text & "' and IncomeTypeId=-1 and DayDate='" & bm.ToStrDate(DayDate.SelectedDate) & "'") Then
            If Not bm.ShowDeleteMSG("تم تجديد الاشتراك اليوم. هل تريد التجديد مرة أخرى؟") Then
                Return
            End If
        End If

        If ProviderId.SelectedIndex < 0 Then ProviderId.SelectedIndex = 0
        If ProviderRateId.SelectedIndex < 0 Then ProviderRateId.SelectedIndex = 0

        DelivaryCost.Text = Val(DelivaryCost.Text)
        Qty.Text = Val(Qty.Text)
        Price.Text = Val(Price.Text)
        Value.Text = Val(Value.Text)
        Payed.Text = Val(Payed.Text)
        Remaining.Text = Val(Remaining.Text)

        
        'If Time.Text = "" Then Time.Text = bm.MyGetTime.TimeOfDay.ToString
        If Time.Text = "" Then Time.Text = bm.MyGetTime.ToString.Substring(10)



        If txtID.Text.Trim = "" Then
            txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName & " where " & MainId & "='" & CboMain.SelectedValue.ToString & "' and " & MainId2 & "='" & CboMain2.SelectedValue.ToString & "'")
            If txtID.Text = "" Then txtID.Text = "1"
        End If


        bm.DefineValues()
        If Not bm.Save(New String() {MainId, MainId2, SubId}, New String() {CboMain.SelectedValue.ToString, CboMain2.SelectedValue.ToString, txtID.Text.Trim}) Then Return
        'bm.ExcuteNonQuery("update Customers set CustomerStatusId='" & CustomerStatusId.SelectedValue.ToString & "',Notes='" & Notes.Text.Trim & "' where Id=" & CustId.Text.Trim())
        bm.ExcuteNonQuery("ChangeCustomerStatus", {"EmpId", "CustId", "CustomerStatusId", "Notes"}, {Md.UserName, CustId.Text.Trim(), CustomerStatusId.SelectedValue.ToString, Notes.Text.Trim})

        If sender Is btnPrint Then
            PrintPone()
        End If
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
            IncomeTypeId_LostFocus(Nothing, Nothing)
        Catch
        End Try
    End Sub

    Sub ClearControls()
        lop = True
        Try
            NewId()
            bm.ClearControls(False)
            CustomerStatusId.SelectedIndex = 0
            DayDate.SelectedDate = bm.MyGetDate()
            DelivaryCost.Clear()
            CustName.Clear()
            Qty.Text = 1
            IncomeTypeId_LostFocus(Nothing, Nothing)
            'txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName & " where " & MainId & "='" & CboMain.SelectedValue.ToString & "' and " & MainId2 & "='" & CboMain2.SelectedValue.ToString & "'")
            'If txtID.Text = "" Then txtID.Text = "1"
            txtID.Clear()
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

    Private Sub DelivaryCost_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Value.KeyDown
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
        UndoNewId()
        bm.FillControls()
        CustId_LostFocus(Nothing, Nothing)
        IncomeTypeId_LostFocus(Nothing, Nothing)
        lop = False
    End Sub

    Private Sub CustId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles CustId.KeyUp
        If bm.ShowHelp("Customers", CustId, CustName, e, "select Top 10 cast(Id as varchar(100)) Id,Name from Customers") Then
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
        bm.LostFocus(CustId, Bal0, "select dbo.CustMotionBal0(" & CustId.Text.Trim() & " ,getdate())")
        ProviderId.SelectedValue = Val(bm.ExecuteScalar("select ProviderId from Customers where Id=" & CustId.Text.Trim()))
        ProviderId_LostFocus(Nothing, Nothing)
        ProviderRateId.SelectedValue = Val(bm.ExecuteScalar("select ProviderRateId from Customers where Id=" & CustId.Text.Trim()))
        ProviderPromotionId.SelectedValue = Val(bm.ExecuteScalar("select ProviderPromotionId from Customers where Id=" & CustId.Text.Trim()))
        CustomerStatusId.SelectedValue = Val(bm.ExecuteScalar("select CustomerStatusId from Customers where Id=" & CustId.Text.Trim()))
        CustBranchId.SelectedValue = Val(bm.ExecuteScalar("select BranchId from Customers where Id=" & CustId.Text.Trim()))
        IncomeTypeId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub IncomeTypeId_LostFocus(sender As Object, e As RoutedEventArgs) Handles IncomeTypeId.LostFocus

        Try
            Qty.IsEnabled = True
            Price.IsEnabled = True
            IsPrev.Visibility = Windows.Visibility.Hidden
            If IncomeTypeId.SelectedValue = -1 Then
                Qty.IsEnabled = False
                Qty.Text = 1
                Price.IsEnabled = Md.Manager
                RechargeDate.Visibility = Windows.Visibility.Visible
                lblRechargeDate.Visibility = Windows.Visibility.Visible
            ElseIf IncomeTypeId.SelectedValue = -2 Then
                Qty.IsEnabled = False
                Qty.Text = 1
                Price.IsEnabled = Md.Manager
                RechargeDate.Visibility = Windows.Visibility.Visible
                lblRechargeDate.Visibility = Windows.Visibility.Visible
                IsPrev.Visibility = Windows.Visibility.Visible
            Else
                RechargeDate.Visibility = Windows.Visibility.Hidden
                lblRechargeDate.Visibility = Windows.Visibility.Hidden
            End If
        Catch
        End Try


        If lop Then Return
        Try
            Qty.IsEnabled = True
            Price.IsEnabled = True
            IsPrev.Visibility = Windows.Visibility.Hidden
            If IncomeTypeId.SelectedValue = -1 Then
                Qty.IsEnabled = False
                Price.IsEnabled = Md.Manager
                CustomerStatusId.SelectedValue = 1
                RechargeDate.Visibility = Windows.Visibility.Visible
                lblRechargeDate.Visibility = Windows.Visibility.Visible
                ActivateDate.SelectedDate = bm.ExecuteScalar("select dbo.GetCustRechargeDate('" & CustId.Text.Trim & "')")
                RechargeDate.SelectedDate = ActivateDate.SelectedDate.Value.AddMonths(1)
            ElseIf IncomeTypeId.SelectedValue = -2 Then
                Qty.IsEnabled = False
                Price.IsEnabled = Md.Manager
                CustomerStatusId.SelectedValue = 1
                RechargeDate.Visibility = Windows.Visibility.Visible
                lblRechargeDate.Visibility = Windows.Visibility.Visible
                ActivateDate.SelectedDate = bm.ExecuteScalar("select dbo.GetCustRechargeDate('" & CustId.Text.Trim & "')")
                RechargeDate.SelectedDate = ActivateDate.SelectedDate.Value.AddMonths(1)
                IsPrev.Visibility = Windows.Visibility.Visible
            Else
                RechargeDate.Visibility = Windows.Visibility.Hidden
                lblRechargeDate.Visibility = Windows.Visibility.Hidden
            End If
            RechargeDate_SelectedDateChanged(Nothing, Nothing)
        Catch
        End Try
    End Sub

    Private Sub RechargeDate_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs) Handles RechargeDate.SelectedDateChanged, ProviderRateId.SelectionChanged
        Try
            If lop Then Return
            If ProviderRateId.SelectedIndex < 1 Then Return
            If RechargeDate.SelectedDate Is Nothing Then Return
            If RechargeDate.SelectedDate < ActivateDate.SelectedDate Then
                'bm.ShowMSG("هذا التاريخ غير صحيح")
                RechargeDate.SelectedDate = ActivateDate.SelectedDate
                Return
            End If

            Dim MyPrice As Decimal = Val(bm.ExecuteScalar("select Price from ProviderRates where ProviderId='" & ProviderId.SelectedValue.ToString & "' and Id='" & ProviderRateId.SelectedValue.ToString & "'"))

            Dim x As Decimal = MyPrice
            dt = bm.ExcuteAdapter("select * from ProviderPromotions where ProviderId='" & ProviderId.SelectedValue.ToString & "' and Id='" & ProviderPromotionId.SelectedValue.ToString & "'")

            Dim dd As DateTime = DateTime.Parse(bm.ExecuteScalar("select ActivateDate from Customers where Id='" & CustId.Text & "'"))
            Dim MonthCount As Integer = 0
            If dt.Rows.Count > 0 Then
                MonthCount = Val(dt.Rows(0)("MonthCount"))
            End If

            If dt.Rows.Count = 0 OrElse DateAdd(DateInterval.Month, MonthCount, dd) <= ActivateDate.SelectedDate Then
                x *= DateDiff(DateInterval.Day, ActivateDate.SelectedDate.Value, RechargeDate.SelectedDate.Value) / DateTime.DaysInMonth(ActivateDate.SelectedDate.Value.Year, ActivateDate.SelectedDate.Value.Month)
            Else
                If DateAdd(DateInterval.Month, 1, ActivateDate.SelectedDate.Value) = RechargeDate.SelectedDate.Value Then
                    x = MyPrice * DateDiff(DateInterval.Month, ActivateDate.SelectedDate.Value, DateAdd(DateInterval.Month, 1, New DateTime(ActivateDate.SelectedDate.Value.Year, ActivateDate.SelectedDate.Value.Month, 1))) * (100 - dt.Rows(0)("Perc")) / 100
                Else
                    x = MyPrice * DateDiff(DateInterval.Day, ActivateDate.SelectedDate.Value, DateAdd(DateInterval.Month, 1, New DateTime(ActivateDate.SelectedDate.Value.Year, ActivateDate.SelectedDate.Value.Month, 1))) / DateTime.DaysInMonth(ActivateDate.SelectedDate.Value.Year, ActivateDate.SelectedDate.Value.Month)

                    x += MyPrice * DateDiff(DateInterval.Month, ActivateDate.SelectedDate.Value, DateAdd(DateInterval.Month, 1, New DateTime(ActivateDate.SelectedDate.Value.Year, ActivateDate.SelectedDate.Value.Month, 1))) * (100 - dt.Rows(0)("Perc")) / 100
                End If
            End If

            x += Val(bm.ExecuteScalar("select IpValue from Customers where Id=" & CustId.Text.Trim()))

            If IncomeTypeId.SelectedValue = -2 Then
                Price.Text = 0
                Payed.Text = 0
            Else
                Price.Text = Math.Round(x, 2, MidpointRounding.AwayFromZero)
            End If
        Catch
        End Try
    End Sub


    Private Sub CustomerStatusId_LostFocus(sender As Object, e As RoutedEventArgs) Handles CustomerStatusId.LostFocus
        If Not Md.Manager Then
            If Val(bm.ExecuteScalar("select Stopped from CustomerStatuses where Id=" & CustomerStatusId.SelectedValue)) Then
                bm.ShowMSG("برجاء اختيار حالة أخرى أو العودة لمدير النظام")
                CustomerStatusId.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub Value_TextChanged(sender As Object, e As TextChangedEventArgs) Handles Value.TextChanged
        If Val(Value.Text) + Val(Bal0.Text) > 0 Then Payed.Text = Val(Value.Text) + Val(Bal0.Text)
    End Sub

    Private Sub Payed_TextChanged(sender As Object, e As TextChangedEventArgs) Handles Payed.TextChanged
        Remaining.Text = Val(Value.Text) + Val(Bal0.Text) - Val(Payed.Text)
    End Sub

    Private Sub PrintPone()
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@BranchId", "@EmpId", "@InvoiceNo", "Header"}
        rpt.paravalue = New String() {CboMain.SelectedValue.ToString, CboMain2.SelectedValue.ToString, Val(txtID.Text), CType(Parent, Page).Title}
        rpt.Rpt = "InvoicesOne.rpt"
        rpt.Print()
    End Sub

    Private Sub Price_TextChanged(sender As Object, e As TextChangedEventArgs) Handles Price.TextChanged, Qty.TextChanged
        Value.Text = Math.Round(Val(Price.Text), 2, MidpointRounding.AwayFromZero) * Val(Qty.Text)
        Payed.Text = Math.Round(Val(Value.Text) + Val(Bal0.Text), 2, MidpointRounding.AwayFromZero)
    End Sub

    Private Sub IsPrev_Checked(sender As Object, e As RoutedEventArgs) Handles IsPrev.Checked
        DayDate.SelectedDate = DayDate.SelectedDate.Value.AddMonths(-1)
    End Sub
    Private Sub IsPrev_UnChecked(sender As Object, e As RoutedEventArgs) Handles IsPrev.Unchecked
        DayDate.SelectedDate = DayDate.SelectedDate.Value.AddMonths(1)
    End Sub

End Class

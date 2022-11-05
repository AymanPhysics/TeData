Imports System.Data
Public Class Customers
    Public TableName As String = "Customers"
    Public SubId As String = "Id"
    Public SubName As String = "Name"

    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.FillCombo("Providers", ProviderId, "", , True)
        bm.FillCombo("CustomerStatuses", CustomerStatusId, "", , True)
        bm.FillCombo(BranchId, "Branches", "Id", "Name")
        BranchId.IsEnabled = Md.Manager

        LoadResource()
        bm.Fields = New String() {SubId, SubName, "MainName", "Address", "Floor", "Appartment", "Mobile", "ProviderId", "ProviderRateId", "BranchId", "DayDate", "ActivateDate", "RechargeDate", "Bal0", "Notes", "WorkOrder", "ServiceUserName", "ServicePassword", "NoOfIp", "CustomerStatusId", "ProviderPromotionId", "IpValue", "NationalID"}
        bm.control = New Control() {txtID, txtName, MainName, Address, Floor, Appartment, Mobile, ProviderId, ProviderRateId, BranchId, DayDate, ActivateDate, RechargeDate, Bal0, Notes, WorkOrder, ServiceUserName, ServicePassword, NoOfIp, CustomerStatusId, ProviderPromotionId, IpValue, NationalID}

        ActivateDate.IsEnabled = Md.Manager
        RechargeDate.IsEnabled = Md.Manager
        btnDelete.IsEnabled = Md.Manager

        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName
        btnNew_Click(sender, e)
        txtID.FlowDirection = Windows.FlowDirection.LeftToRight
        ServiceUserName.FlowDirection = Windows.FlowDirection.LeftToRight
        ServicePassword.FlowDirection = Windows.FlowDirection.LeftToRight
        txtID.HorizontalContentAlignment = Windows.HorizontalAlignment.Left
        ServiceUserName.HorizontalContentAlignment = Windows.HorizontalAlignment.Left
        ServicePassword.HorizontalContentAlignment = Windows.HorizontalAlignment.Left
    End Sub

    Sub FillControls()
        bm.FillControls()
        ProviderId_LostFocus(Nothing, Nothing)
        bm.FillControls()
        If Not Md.CustEdit AndAlso Not Md.BranchId = BranchId.SelectedValue Then
            bm.ShowMSG("لا يمكن التعامل علي هذا العميل")
            btnNew_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnPrint.Click
        If txtName.Text.Trim = "" Then
            txtName.Focus()
            Return
        End If

        If ProviderId.SelectedIndex < 0 Then ProviderId.SelectedIndex = 0
        If ProviderRateId.SelectedIndex < 0 Then ProviderRateId.SelectedIndex = 0

        DescPerc.Text = Val(DescPerc.Text.Trim)
        IpValue.Text = Val(IpValue.Text.Trim)
        Bal0.Text = Val(Bal0.Text.Trim)

        bm.ExcuteNonQuery("ChangeCustomerStatus", {"EmpId", "CustId", "CustomerStatusId", "Notes"}, {Md.UserName, txtID.Text.Trim(), CustomerStatusId.SelectedValue.ToString, Notes.Text.Trim})

        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return

        If sender Is btnPrint Then
            PrintPone()
        Else
            btnNew_Click(sender, e)
        End If

    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        bm.ClearControls()
        ClearControls()
        txtID.Focus()
    End Sub

    Sub ClearControls()
        bm.ClearControls()
        CustomerStatusId.SelectedValue = 3
        Dim MyNow As DateTime = bm.MyGetDate()
        DayDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        'ActivateDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        'RechargeDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        BranchId.SelectedValue = Md.BranchId

        txtName.Clear()
        txtID.Text = "03"
        txtID.SelectionStart = 2
        
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExcuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "'")
            btnNew_Click(sender, e)
        End If
    End Sub

    Dim lv As Boolean = False

    Private Sub txtID_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {SubId}, New String() {txtID.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            Dim s As String = txtID.Text
            ClearControls()
            txtID.Text = s
            txtName.Focus()
            lv = False
            Return
        End If
        FillControls()
        lv = False
        txtName.SelectAll()
        txtName.Focus()
        txtName.SelectAll()
        txtName.Focus()
        'txtName.Text = dt(0)("Name")
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles DescPerc.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub


    Private Sub LoadResource()

    End Sub

    Private Sub txtID_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyUp
        If bm.ShowHelp("العملاء", txtID, txtName, e, "Select Id,Name From Customers", , , , "التليفون", ) Then
            txtID_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub ProviderId_LostFocus(sender As Object, e As RoutedEventArgs) Handles ProviderId.LostFocus
        If ProviderId.SelectedIndex < 0 Then Return
        bm.FillCombo("ProviderRates", ProviderRateId, "where ProviderId='" & ProviderId.SelectedValue.ToString & "'", , True)
        bm.FillCombo("ProviderPromotions", ProviderPromotionId, "where ProviderId='" & ProviderId.SelectedValue.ToString & "'", , True)

        If ProviderId.SelectedValue = 1 Then
            ActivateDate.IsEnabled = Md.Manager
        Else
            ActivateDate.IsEnabled = True
        End If
    End Sub

    Private Sub ActivateDate_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs) Handles ActivateDate.SelectedDateChanged
        If ActivateDate.SelectedDate Is Nothing Then Return
        RechargeDate.SelectedDate = ActivateDate.SelectedDate.Value.AddMonths(1)
    End Sub

    Private Sub RechargeDate_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs) Handles RechargeDate.SelectedDateChanged, ProviderRateId.SelectionChanged, ProviderPromotionId.SelectionChanged
        If lv Then Return
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
        If dt.Rows.Count = 0 Then
            x *= DateDiff(DateInterval.Day, ActivateDate.SelectedDate.Value, RechargeDate.SelectedDate.Value) / DateTime.DaysInMonth(ActivateDate.SelectedDate.Value.Year, ActivateDate.SelectedDate.Value.Month)
        Else
            If DateAdd(DateInterval.Month, 1, ActivateDate.SelectedDate.Value) > RechargeDate.SelectedDate.Value Then
                x = MyPrice * DateDiff(DateInterval.Day, ActivateDate.SelectedDate.Value, RechargeDate.SelectedDate.Value) / DateTime.DaysInMonth(ActivateDate.SelectedDate.Value.Year, ActivateDate.SelectedDate.Value.Month)
            ElseIf DateAdd(DateInterval.Month, 1, ActivateDate.SelectedDate.Value) = RechargeDate.SelectedDate.Value Then
                x = MyPrice * DateDiff(DateInterval.Month, ActivateDate.SelectedDate.Value, DateAdd(DateInterval.Month, 1, New DateTime(ActivateDate.SelectedDate.Value.Year, ActivateDate.SelectedDate.Value.Month, 1))) * (100 - dt.Rows(0)("Perc")) / 100
            Else
                x = MyPrice * DateDiff(DateInterval.Day, ActivateDate.SelectedDate.Value, DateAdd(DateInterval.Month, 1, New DateTime(ActivateDate.SelectedDate.Value.Year, ActivateDate.SelectedDate.Value.Month, 1))) / DateTime.DaysInMonth(ActivateDate.SelectedDate.Value.Year, ActivateDate.SelectedDate.Value.Month)

                x += MyPrice * DateDiff(DateInterval.Month, ActivateDate.SelectedDate.Value, DateAdd(DateInterval.Month, 1, New DateTime(ActivateDate.SelectedDate.Value.Year, ActivateDate.SelectedDate.Value.Month, 1))) * (100 - dt.Rows(0)("Perc")) / 100
            End If
        End If

        Bal0.Text = Math.Round(x, 2, MidpointRounding.AwayFromZero)

    End Sub

    Private Sub PrintPone()
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@Id", "Header"}
        rpt.paravalue = New String() {txtID.Text, CType(Parent, Page).Title}
        rpt.Rpt = "CustomerOne.rpt"
        rpt.Print()
    End Sub

End Class

Imports System.Data

Public Class RPT1

    Dim MainTableName As String = "Branches"

    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click

        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@ProviderId", "@ProviderRateId", "@ProviderPromotionId", "@BranchId", "@CustomerStatusId", "Header", "@CustId", "@FromDate", "@ToDate"}
        rpt.paravalue = New String() {Val(ProviderId.SelectedValue.ToString), Val(ProviderRateId.SelectedValue.ToString), Val(ProviderPromotionId.SelectedValue.ToString), Val(BranchId.SelectedValue.ToString), Val(CustomerStatusId.SelectedValue.ToString), CType(Parent, Page).Title, CustId.Text.Trim, FromDate.SelectedDate, ToDate.SelectedDate}

        Select Case Flag
            Case 1
                rpt.Rpt = "Customers.rpt"
            Case 2
                rpt.Rpt = "CustomersProfit.rpt"
        End Select
        rpt.ShowDialog()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return

        bm.FillCombo("Providers", ProviderId, "", , True)
        bm.FillCombo("CustomerStatuses", CustomerStatusId, "", , True)
        bm.FillCombo(BranchId, "Branches", "Id", "Name")
        ProviderId_LostFocus(Nothing, Nothing)

        BranchId.SelectedValue = Md.BranchId
        BranchId.IsEnabled = Md.CustView

        bm.Addcontrol_MouseDoubleClick({})

        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)

    End Sub

    
    Private Sub ProviderId_LostFocus(sender As Object, e As RoutedEventArgs) Handles ProviderId.LostFocus
        If ProviderId.SelectedIndex < 0 Then Return
        bm.FillCombo("ProviderRates", ProviderRateId, "where ProviderId='" & ProviderId.SelectedValue.ToString & "'", , True)
        bm.FillCombo("ProviderPromotions", ProviderPromotionId, "where ProviderId='" & ProviderId.SelectedValue.ToString & "'", , True)
    End Sub


End Class

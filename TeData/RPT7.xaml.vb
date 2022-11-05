Imports System.Data

Public Class RPT7
    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Flag As Integer = 0

    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click

        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@FromDate", "@ToDate", "@BranchId", "EmpName", "Header", "@CustId"}
        rpt.paravalue = New String() {FromDate.SelectedDate, ToDate.SelectedDate, Val(BranchId.SelectedValue.ToString), BranchId.Text, CType(Parent, Page).Title, CustId.Text.Trim}
        Select Case Flag
            Case 1
                rpt.Rpt = "CustRechargeDate.rpt"
            Case 2
                rpt.Rpt = "CustRechargeDate2.rpt"
        End Select
        rpt.ShowDialog()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.FillCombo(BranchId, "Branches", "Id", "Name")
        BranchId.SelectedValue = Md.BranchId
        BranchId.IsEnabled = Md.CustView

        Dim MyNow As DateTime = bm.MyGetDate().AddDays(-5)
        FromDate.SelectedDate = New DateTime(2000, 1, 1, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
    End Sub
    



End Class
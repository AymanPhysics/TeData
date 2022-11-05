Imports System.Data

Public Class RPT6

    Dim MainTableName As String = "Branches"
    
    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click

        

        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@CustBranchId", "@BranchId", "@EmpId", "@CustId", "@Done", "@IncomeTypeId",
"@FromDate", "@ToDate", "Header", "@OrderTypeId", "@ProviderId"}
        rpt.paravalue = New String() {Val(CustBranch.SelectedValue.ToString), Val(CboMain.SelectedValue.ToString), Val(CboMain2.SelectedValue.ToString), CustId.Text.Trim, Val(IncomeTypeId.SelectedValue.ToString), Val(IncomeTypeId.SelectedValue.ToString), FromDate.SelectedDate, ToDate.SelectedDate, CType(Parent, Page).Title, Val(OrderTypeId.SelectedValue.ToString), Val(ProviderId.SelectedValue.ToString)}

        Select Case Flag
            Case 1
                rpt.Rpt = "Invoices.rpt"
            Case 2
                rpt.Rpt = "Invoices4.rpt"
            Case 3
                rpt.Rpt = "CustomerStatusMotion.rpt"
        End Select
        rpt.ShowDialog()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return

        bm.FillCombo(MainTableName, CboMain, "")
        bm.FillCombo(MainTableName, CustBranch, "")
        bm.FillCombo("OrderTypes", OrderTypeId, "")
        bm.FillCombo("Providers", ProviderId, "")

        CboMain.IsEnabled = Md.CustView
        CboMain2.IsEnabled = Md.CustView

        Select Flag
            Case 2
                bm.FillCombo("select 0 Id,'لم يتم' Name union select 1 Id,'تم' Name union select 2 Id,'الكل' Name", IncomeTypeId)
                IncomeTypeId.SelectedValue = 2
                lblType.Content = "النوع"
            Case Else
                OrderTypeId.Visibility = Windows.Visibility.Hidden
                lblOrderTypeId.Visibility = Windows.Visibility.Hidden

                bm.FillCombo("select * from(select -1 Id,'** تجديد الاشتراك **' Name union select 0 Id,'-' Name union select cast(Id as varchar(100)) Id,Name from IncomeTypes)tbl order by abs(Id),Id", IncomeTypeId)
        End Select

        If Flag <> 1 Then
            lblProviderId.Visibility = Windows.Visibility.Hidden
            ProviderId.Visibility = Windows.Visibility.Hidden
        End If

        If Flag = 3 Then
            lblType.Visibility = Windows.Visibility.Hidden
            IncomeTypeId.Visibility = Windows.Visibility.Hidden
            lblMain.Visibility = Windows.Visibility.Hidden
            CboMain.Visibility = Windows.Visibility.Hidden
            lblMain2.Visibility = Windows.Visibility.Hidden
            CboMain2.Visibility = Windows.Visibility.Hidden
        End If
        bm.Addcontrol_MouseDoubleClick({CustId})

        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)

    End Sub


    Private Sub CboMain_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboMain.SelectionChanged
        Dim s As String = ""
        Try
            s = CboMain.SelectedValue.ToString
        Catch ex As Exception
        End Try
        bm.FillCombo("select 0 Id, '-' Name union Select Id,ArName Name From Employees where BranchId='" & s & "' order by Name", CboMain2)
    End Sub

    Private Sub CustId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles CustId.KeyUp
        bm.ShowHelp("Customers", CustId, CustName, e, "select cast(Id as varchar(100)) Id,Name from Customers")
        CustId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub CustId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CustId.LostFocus
        bm.LostFocus(CustId, CustName, "select Name from Customers where Id=" & CustId.Text.Trim())
    End Sub
End Class

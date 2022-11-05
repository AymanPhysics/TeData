Imports System.Data

Public Class RPT8

    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click



        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@CustId", "@ToDate", "Header"}
        rpt.paravalue = New String() {Val(CustId.Text.Trim), ToDate.SelectedDate, CType(Parent, Page).Title}

        Select Case Flag
            Case 1
                rpt.Rpt = "CustMotion.rpt"
            Case 2
                rpt.Rpt = "EmpAllBal.rpt"
        End Select
        rpt.ShowDialog()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return

        If Flag = 2 Then
            lblCustId.Visibility = Windows.Visibility.Hidden
            CustId.Visibility = Windows.Visibility.Hidden
            CustName.Visibility = Windows.Visibility.Hidden
        End If

        bm.Addcontrol_MouseDoubleClick({CustId})

        Dim MyNow As DateTime = bm.MyGetDate()
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)

    End Sub


    Private Sub CustId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles CustId.KeyUp
        bm.ShowHelp("Customers", CustId, CustName, e, "select cast(Id as varchar(100)) Id,Name from Customers")
        CustId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub CustId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CustId.LostFocus
        bm.LostFocus(CustId, CustName, "select Name from Customers where Id=" & CustId.Text.Trim())
    End Sub
End Class

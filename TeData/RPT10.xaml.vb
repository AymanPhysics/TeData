Imports System.Data

Public Class RPT10

    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click



        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@EmpId", "@FromDate", "@ToDate", "Header"}
        rpt.paravalue = New String() {Val(EmpId.Text.Trim), FromDate.SelectedDate, ToDate.SelectedDate, CType(Parent, Page).Title}

        Select Case Flag
            Case 1
                rpt.Rpt = "EmpMotion.rpt"
        End Select
        rpt.ShowDialog()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return


        bm.Addcontrol_MouseDoubleClick({EmpId})

        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)

    End Sub


    Private Sub EmpId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles EmpId.KeyUp
        bm.ShowHelp("Employees", EmpId, EmpName, e, "select cast(Id as varchar(100)) Id,ArName Name from Employees")
        EmpId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub EmpId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles EmpId.LostFocus
        bm.LostFocus(EmpId, EmpName, "select ArName Name from Employees where Id=" & EmpId.Text.Trim())
    End Sub
End Class

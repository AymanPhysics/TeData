Imports System.Data

Imports System
Imports System.Collections
Imports System.ComponentModel

Imports System.Drawing
Imports System.Text
Imports System.Drawing.Imaging
 
Imports System.IO
 
Imports TCPCamActivex

Public Class PrintReset

    Public Flag As Integer = 0
    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Sub PrintReset_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        Dim MyNow As DateTime = bm.MyGetDate()
        DayDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)

        V1.Text = 1
        V2.Text = 100

    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles SubNoAreaId.KeyDown, TypeId.KeyDown, TypeId.KeyDown, TypeId.KeyDown, TypeId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs)
        bm.MyKeyPress(sender, e, True)
    End Sub

    Private Sub Button2_Click(sender As Object, e As RoutedEventArgs) Handles Button2.Click
        For i As Integer = Val(V1.Text) To Val(V2.Text)
            PrintReset(i)
        Next
    End Sub

    Public Sub PrintReset(i As Integer)
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"Id", "Flag", "DayDate", "UserName", "Header", "BtnName", "lbl"}
        rpt.paravalue = New String() {i, "", DayDate.SelectedDate, Md.UserName, "", "", ""}
        rpt.Rpt = "Reset.rpt"
        rpt.Print()
    End Sub

End Class

' Copyright (c) 2018 Erik Colley

' https://github.com/ejj28/tasky

' This file is part of Tasky.

' Tasky is free software: you can redistribute it and/or modify
' it under the terms of the GNU Affero General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.

' Tasky is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU Affero General Public License for more details.

' You should have received a copy of the GNU Affero General Public License
' along with Tasky.  If not, see <http://www.gnu.org/licenses/>.

Imports System.Diagnostics

Public Class Form1

    Dim version = "1.0.0"

    Dim cpu As New PerformanceCounter()
    Dim datavalues As New List(Of Integer)
    Dim cpuVals As New List(Of Integer)
    Dim timerIter As New Integer
    Dim cpuAvg As New Integer

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Label5.Text = "Version " + version

        Form2.TextBox1.Text = My.Resources.COPYING.ToString

        refreshlist()

        For i = 0 To 20
            datavalues.Add(0)
        Next

        With cpu
            .CategoryName = "Processor"
            .CounterName = "% Processor Time"
            .InstanceName = "_Total"
        End With

        Chart1.ChartAreas(0).AxisY.Maximum = 100

        Label2.Text = cpu.NextValue()

        Dim getInfo As New Process
        getInfo.StartInfo.FileName = "systeminfo"
        getInfo.StartInfo.UseShellExecute = False
        getInfo.StartInfo.RedirectStandardInput = True
        getInfo.StartInfo.RedirectStandardOutput = True
        getInfo.StartInfo.CreateNoWindow = True
        getInfo.Start()
        Dim output As System.IO.StreamReader = getInfo.StandardOutput

        TextBox2.Text = output.ReadToEnd

        output.Close()
        getInfo.Close()

        Dim textLines = TextBox2.Lines

        For Each i In textLines
            ListBox2.Items.Add(i.ToString)
        Next







    End Sub

    Sub refreshlist()

        ListView2.Items.Clear()

        Dim proc(4) As String
        Dim itm As ListViewItem

        For Each p As Process In Process.GetProcesses
            proc(0) = p.ProcessName.ToString
            proc(1) = p.Responding.ToString
            proc(2) = p.Id.ToString
            proc(3) = (p.WorkingSet64 / 1024).ToString & "K"
            itm = New ListViewItem(proc)
            ListView2.Items.Add(itm)

        Next

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        System.Diagnostics.Process.GetProcessesByName(ListView2.FocusedItem.Text)(0).Kill()
        refreshlist()
    End Sub

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        refreshlist()
    End Sub



    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick


        cpuVals.Add(cpu.NextValue)



        If timerIter = 10 Then
            Dim t As Integer
            timerIter = 1
            For Each t In cpuVals
                cpuAvg = cpuAvg + t
            Next
            cpuAvg = cpuAvg / 10
            datavalues.Add(cpuAvg)
            cpuVals.Clear()

            Chart1.Series("Series1").Points.Clear()
            If datavalues.Count > 20 Then
                datavalues.RemoveAt(0)
            End If
            Dim i As Integer
            Dim count = 0
            For Each i In datavalues
                Chart1.Series("Series1").Points.AddXY(count, i)
                count = count + 1
            Next
            Label2.Text = cpuAvg.ToString + "%"
        Else
            timerIter = timerIter + 1
        End If

    End Sub

    Private Sub LinkLabel1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkLabel1.Click
        System.Diagnostics.Process.Start("https://www.github.com/ejj28")
    End Sub

    Private Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.Click
        SaveFileDialog1.ShowDialog()



    End Sub

    Private Sub SaveFileDialog1_FileOk(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        Dim exportPath = SaveFileDialog1.FileName
        If Not System.IO.File.Exists(exportPath.ToString) = True Then
            Dim file As System.IO.FileStream
            file = System.IO.File.Create(exportPath.ToString)
            file.Close()
        End If
        My.Computer.FileSystem.WriteAllText(exportPath, TextBox2.Text, False)
        MsgBox("Export Complete", MsgBoxStyle.OkOnly, "Tasky Export")
    End Sub

    Private Sub Button4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button4.Click
        Form2.Show()
    End Sub

    
    Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Process.Start("https://github.com/ejj28/tasky")
    End Sub
End Class

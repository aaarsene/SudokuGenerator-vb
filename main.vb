Imports System

Module Module1

  Sub Main()
    Dim sudoku9 As New Sudoku(9)
    sudoku9.Display()
    Console.WriteLine("---------------")
    Dim sudoku4 As New Sudoku(4)
    sudoku4.Display()
  End Sub

End Module

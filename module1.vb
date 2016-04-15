Imports System

Module Module1

  Sub Main()
    Dim sudoku As New Sudoku(9)
    Dim revealed As Integer
    Console.Write("Nombre de cases à révéler (17-81): ")
    revealed = CInt(Console.ReadLine())
    sudoku.Reveal(revealed)
    sudoku.Display()

    While Not sudoku.Solved()
      Console.WriteLine("Entrez les infos sur la case à tester")
      Console.Write(" Ligne (1-9) : ")
      Dim row As Integer = CInt(Console.ReadLine())
      Console.Write(" Colone (1-9) : ")
      Dim col As Integer = CInt(Console.ReadLine())
      Console.Write(" Valeur (1-9) : ")
      Dim value As Integer = CInt(Console.ReadLine())
      sudoku.AddUserNumber(row,col,value)
      sudoku.Display()
    End While
  End Sub

End Module

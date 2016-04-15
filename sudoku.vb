Imports System
Imports System.Collections.Generic

Class Sudoku
  Private Size As Integer
  Private Grid(,) As Integer
  Private Revealed(,) As Boolean
  Private UserGrid(,) As Integer
  Private History As New List(Of Tuple(Of Integer, Integer, List(Of Integer)))

  Sub New(ByVal size As Integer)
    Me.Size = size
    ReDim Me.Grid(size-1,size-1)
    ReDim Me.Revealed(size-1,size-1)
    ReDim Me.UserGrid(size-1,size-1)
    Me.ClearGrid()
    Me.Generate()
  End Sub

  Public Sub Display()
  Console.WriteLine()
  For row As Integer = 0 To Me.Size - 1
    Console.Write(" ")
    For col As Integer = 0 To Me.Size - 1
      If Me.Revealed(row,col) Then
        Console.Write(Me.Grid(row,col))
      Else If Me.UserGrid(row,col) > 0 Then
        Console.ForegroundColor = ConsoleColor.White
        Console.Write(Me.UserGrid(row,col))
        Console.ResetColor()
      Else
        Console.ForegroundColor = ConsoleColor.DarkGray
        Console.Write("#")
        Console.ResetColor()
      End If
      If col Mod Me.SquareSize() = Me.SquareSize()-1 Then
        Console.Write(" ")
      End If
    Next
    If row Mod Me.SquareSize() = Me.SquareSize()-1 Then
      Console.WriteLine()
    End If
    Console.WriteLine()
  Next
  End Sub



  'Fonctions pour générer la grille

  Public Sub Generate()
    While Me.Zeros()
      Me.Fill()
      If Me.Zeros() Then
        Me.Rewind()
      End If
    End While
  End Sub

  Public Function Fill() As Boolean
    Dim deleted As Boolean = False
    If Me.History.Count > 0 Then
      If Me.Grid(Me.History(Me.History.Count-1).Item1, _
                 Me.History(Me.History.Count-1).Item2) = 0 Then
        deleted = True
      End If
    End If

    For r As Integer = 0 To Me.Size - 1
      For c As Integer = 0 To Me.Size - 1
        Dim cell As Integer = Me.Grid(r,c)
        If cell = 0 Then
          Dim possibles As List(Of Integer) = New List(Of Integer)

          For i As Integer = 1 To Me.Size
            If Me.LegalMove(r,c,i) Then
              possibles.Add(i)
            End If
          Next

          If deleted Then
            possibles = Me.History(Me.History.Count-1).Item3
            possibles = possibles.GetRange(1,possibles.Count-1)
            Me.History = Me.History.GetRange(0,Me.History.Count-1)
            deleted = False
          End If

          Shuffle(possibles)
          If possibles.Count > 0 Then
            Me.Grid(r,c) = possibles(0)
            Dim h As Tuple(Of Integer, Integer, List(Of Integer)) _
              = New Tuple(Of Integer, Integer, List(Of Integer)) _
                (r,c,possibles)
            Me.History.Add(h)
          Else
            Return True
          End If
        End If
      Next
    Next
    Return False
  End Function

  Private Sub Rewind()
    Dim lastItem As Integer = Me.History.Count
    For index As Integer = 0 To Me.History.Count-1
      If Me.History(index).Item3.Count > 1 Then
        lastItem = index
      End If
    Next

    Me.ClearFrom(Me.History(lastItem).Item1, Me.History(lastItem).Item2)
    Me.History = Me.History.GetRange(0,lastItem+1)
  End Sub

  Private Function InRow(ByVal row As Integer, ByVal value As Integer) As Boolean
    For col As Integer = 0 To Me.Size - 1
      If value = Me.Grid(row,col) Then
        Return True
      End If
    Next
    Return False
  End Function

  Private Function InCol(ByVal col As Integer, ByVal value As Integer) As Boolean
    For row As Integer = 0 To Me.Size - 1
      If value = Me.Grid(row,col) Then
        Return True
      End If
    Next
    Return False
  End Function

  Private Function InSquare(ByVal row As Integer, _
                            ByVal col As Integer, _
                            ByVal value As Integer) As Boolean

    For r As Integer = 0 To Me.Size - 1
      For c As Integer = 0 To Me.Size - 1
        If r >= Math.Floor(row/Me.SquareSize())*Me.SquareSize() And r < Math.Floor(((row/Me.SquareSize())+1))*Me.SquareSize() Then
          If c >= Math.Floor(col/Me.SquareSize())*Me.SquareSize() And c < Math.Floor(((col/Me.SquareSize())+1))*Me.SquareSize() Then
            If value = Me.Grid(r,c) Then
              Return True
            End If
          End If
        End If
      Next
    Next
    Return False
  End Function

  Private Function LegalMove(ByVal row As Integer, _
                            ByVal col As Integer, _
                            ByVal value As Integer) As Boolean
    If Not Me.InRow(row, value) _
        And Not Me.InCol(col, value) _
        And Not Me.InSquare(row, col, value)
      Return True
    Else
      Return False
    End If
  End Function

  Private Function Zeros() As Boolean
    For row As Integer = 0 To Me.Size - 1
      For col As Integer = 0 To Me.Size - 1
        If Me.Grid(row,col) = 0 Then
          Return True
        End If
      Next
    Next
    Return False
  End Function

  Private Sub ClearGrid()
    For row As Integer = 0 To Me.Size - 1
      For col As Integer = 0 To Me.Size - 1
        Me.Grid(row,col) = 0
      Next
    Next
  End Sub

  Private Sub ClearFrom(ByVal row As Integer, ByVal col As Integer)
    For r As Integer = 0 To Me.Size - 1
      For c As Integer = 0 To Me.Size - 1
        If r > row Then
          Me.Grid(r,c) = 0
        ElseIf r = row And c >= col Then
          Me.Grid(r,c) = 0
        End If
      Next
    Next
  End Sub

  Private Function SquareSize() As Integer
    Return Math.Sqrt(Me.Size)
  End Function



  'Fonctions pour pouvoir jouer avec la grille
  Public Sub RevealAll()
    For row As Integer = 0 To Me.Size - 1
      For col As Integer = 0 To Me.Size - 1
        Me.Revealed(row,col) = True
      Next
    Next
  End Sub

  Public Sub Reveal(ByVal count As Integer)
    If count <= Me.Size*Me.Size Or count >= 17 Then
      For i As Integer = 1 To count
        Dim cell As Tuple(Of Integer, Integer) = Me.GetRandomHidden()
        Me.Revealed(cell.Item1,cell.Item2) = True
      Next
    End If
  End Sub

  Private Function GetRandomHidden() As Tuple(Of Integer, Integer)
    Dim hidden As New List(Of Tuple(Of Integer, Integer))
    For r As Integer = 0 To Me.Size - 1
    For c As Integer = 0 To Me.Size - 1
      If Not Me.Revealed(r,c) Then
        Dim cell As Tuple(Of Integer, Integer) _
              = New Tuple(Of Integer, Integer) _
                (r,c)
        hidden.Add(cell)
      End If
    Next
    Next
    Return hidden(Me.GetRandom(0,hidden.Count-1))
  End Function

  Public Sub AddUserNumber(ByVal row As Integer, _
                            ByVal col As Integer, _
                            ByVal value As Integer)

    If Me.Revealed(row-1,col-1) Then
      Console.WriteLine("Cette case est déjà révélée")
    Else If Me.Grid(row-1,col-1) <> value Then
      Console.WriteLine("Ce mouvement n'est pas autorisé")
    Else
      Me.UserGrid(row-1,col-1) = value
    End If
  End Sub

  Public Function Solved() As Boolean
    For row As Integer = 0 To Me.Size - 1
      For col As Integer = 0 To Me.Size - 1
        If Me.Revealed(row,col) = False And Me.UserGrid(row,col) = 0 Then
          Return False
        End If
      Next
    Next
    Return True
  End Function

  'Fonctions pour générer de l'aléatoire

  Private Function GetRandom(ByVal Min As Integer, ByVal Max As Integer) As Integer
    Static Generator As System.Random = New System.Random()
    Return Generator.Next(Min, Max)
  End Function

  Private Sub Shuffle(ByVal list As List(Of Integer))
    For i As Integer = 0 To list.Count - 1
        Dim index As Integer = Me.GetRandom(i, list.Count)
        If i <> index Then
            ' swap list(i) and list(index)
            Dim temp As Integer = list(i)
            list(i) = list(index)
            list(index) = temp
        End If
    Next
  End Sub
End Class

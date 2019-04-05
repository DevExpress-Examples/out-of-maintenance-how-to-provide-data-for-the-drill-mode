Public Class MainViewModel
    Dim _branches As IReadOnlyList(Of DevAVBranch)
    Public ReadOnly Property Branches As IReadOnlyList(Of DevAVBranch)
        Get
            Return _branches
        End Get
    End Property

    Public Sub New()
        _branches = New BranchDAO().GetBranches()
    End Sub

End Class

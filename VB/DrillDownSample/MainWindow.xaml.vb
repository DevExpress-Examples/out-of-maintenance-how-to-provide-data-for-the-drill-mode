Imports System.Text
Imports DevExpress.Xpf.Charts

Class MainWindow
    Private Sub Diagram_DrillDownStateChanged(sender As Object, e As DrillDownStateChangedEventArgs)
        diagram.Rotated = e.BreadcrumbItems.Last().IsHome
    End Sub
End Class

Public Class DevAVSeriesChildrenSelector
    Implements IChildrenSelector

    Public Function SelectChildren(item As Object) As IEnumerable Implements IChildrenSelector.SelectChildren
        Dim branch = TryCast(item, DevAVBranch)
        If (branch IsNot Nothing) Then Return New List(Of DevAVBranch) From {branch}

        Dim category = TryCast(item, DevAVProductCategory)
        If (category IsNot Nothing) Then Return category.Products

        Dim product = TryCast(item, DevAVProduct)
        If (product IsNot Nothing) Then Return New List(Of DevAVProduct) From {product}

        Return Nothing
    End Function
End Class

Public Class DevAVSeriesTemplateSelector
    Inherits DataTemplateSelector

    Public Property AllCategoriesTemplate As DataTemplate
    Public Property BranchCategoriesTemplate As DataTemplate
    Public Property CategoryProductsTemplate As DataTemplate
    Public Property ProductTemplate As DataTemplate

    Public Overrides Function SelectTemplate(item As Object, container As DependencyObject) As DataTemplate
        Dim diagram = CType(container, Diagram)

        Dim branch = TryCast(item, DevAVBranch)
        If (branch IsNot Nothing) And (diagram.BreadcrumbItems.Count = 1) Then Return AllCategoriesTemplate
        If (branch IsNot Nothing) Then Return BranchCategoriesTemplate

        Dim product = TryCast(item, DevAVProduct)
        Dim lastLevelSourceProduct = TryCast(diagram.BreadcrumbItems(diagram.BreadcrumbItems.Count - 1).SourceObject, DevAVProduct)
        If (product IsNot Nothing) And (lastLevelSourceProduct IsNot Nothing) Then Return ProductTemplate
        If (product IsNot Nothing) Then Return CategoryProductsTemplate

        Return Nothing
    End Function

End Class

Public Class DevAVBreadcrumbTextProvider
    Implements IBreadcrumbTextProvider
    Public Function GetText(seriesSourceObj As Object, pointSourceObj As Object) As String Implements IBreadcrumbTextProvider.GetText
        Dim sb = New StringBuilder("(")
        If (seriesSourceObj IsNot Nothing) Then sb.Append(GetName(seriesSourceObj))
        If (seriesSourceObj IsNot Nothing And pointSourceObj IsNot Nothing) Then sb.Append(", ")
        If (pointSourceObj IsNot Nothing) Then sb.Append(GetName(pointSourceObj))
        sb.Append(")")
        Return sb.ToString()
    End Function

    Private Function GetName(obj As Object) As String
        Dim nameable = TryCast(obj, INameable)
        If (nameable IsNot Nothing) Then Return nameable.Name
        Return String.Empty
    End Function
End Class
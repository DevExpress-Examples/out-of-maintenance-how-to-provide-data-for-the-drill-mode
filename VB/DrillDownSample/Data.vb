Imports DrillDownSample

Public Interface INameable
    ReadOnly Property Name As String
End Interface

Public Class DevAVBranch
    Implements INameable

    Public Property Name As String Implements INameable.Name
    Public Property Categories As List(Of DevAVProductCategory)
    Public ReadOnly Property TotalIncome As Double
        Get
            Return Categories.Sum(Function(c) c.TotalIncome)
        End Get
    End Property
End Class

Public Class DevAVProductCategory
    Implements INameable

    Public Property Name As String Implements INameable.Name
    Public Property Products As List(Of DevAVProduct)
    Public ReadOnly Property TotalIncome As Double
        Get
            Return Products.Sum(Function(p) p.TotalIncome)
        End Get
    End Property
End Class

Public Class DevAVProduct
    Implements INameable

    Public Property Name As String Implements INameable.Name
    Public Property Sales As List(Of DevAVMonthlyIncome)
    Public ReadOnly Property TotalIncome As Double
        Get
            Return Sales.Sum(Function(s) s.Income)
        End Get
    End Property
End Class

Public Class DevAVMonthlyIncome
    Public Property Month As DateTime
    Public Property Income As Double
End Class


Public Class BranchDAO
    ReadOnly companies() As String = {
        "DevAV North",
        "DevAV South",
        "DevAV West",
        "DevAV East",
        "DevAV Central"
    }
    ReadOnly categorizedProducts = New Dictionary(Of String, List(Of String)) From {
        {"Cell Phones", New List(Of String) From {"Smartphones", "Mobile Phones", "Smart Watches", "Sim Cards"}},
        {"Computers", New List(Of String) From {"PCs", "Laptops", "Tablets", "Printers"}},
        {"TV, Audio", New List(Of String) From {"TVs", "Home Audio", "Headphones", "DVD Players"}},
        {"Car Electronics", New List(Of String) From {"GPS Units", "Radars", "Car Alarms", "Car Accessories"}},
        {"Power Devices", New List(Of String) From {"Batteries", "Chargers", "Converters", "Testers", "AC/DC Adapters"}},
        {"Photo", New List(Of String) From {"Cameras", "Camcorders", "Binoculars", "Flashes", "Tripodes"}}
    }

    ReadOnly rnd = New Random(2019)
    Dim endDate As DateTime

    Public Sub New()
        Dim now = DateTime.Now
        Me.endDate = New DateTime(Now.Year, Now.Month, 1)
    End Sub

    Public Function GetBranches() As List(Of DevAVBranch)
        Dim data = New List(Of DevAVBranch)()
        For Each branchName As String In Me.companies
            Dim companyFactor As Double = rnd.NextDouble() * 0.6 + 1
            Dim categories As List(Of DevAVProductCategory) = GenerateBranchSales(companyFactor)
            data.Add(
                New DevAVBranch With {
                    .Name = branchName,
                    .Categories = categories
                })
        Next branchName
        Return data
    End Function
    Function GenerateBranchSales(companyFactor As Double) As List(Of DevAVProductCategory)
        Dim categories = New List(Of DevAVProductCategory)()
        For Each categoryProductsPair As KeyValuePair(Of String, List(Of String)) In Me.categorizedProducts
            Dim categoryFactor As Double = rnd.NextDouble() * 0.6 + 1
            Dim products As List(Of DevAVProduct) = GenerateCategoryProducts(categoryProductsPair, companyFactor, categoryFactor)
            categories.Add(New DevAVProductCategory With {
                    .Name = categoryProductsPair.Key,
                    .Products = products
            })
        Next categoryProductsPair
        Return categories
    End Function
    Function GenerateCategoryProducts(categoryProductsPair As KeyValuePair(Of String, List(Of String)), companyFactor As Double, categoryFactor As Double) As List(Of DevAVProduct)
        Dim products = New List(Of DevAVProduct)()
        For Each productName As String In categoryProductsPair.Value
            Dim sales = GenerateSalesForProduct(companyFactor, categoryFactor)
            products.Add(New DevAVProduct With {
                    .Name = productName,
                    .Sales = sales
            })
        Next productName
        Return products
    End Function
    Function GenerateSalesForProduct(companyFactor As Double, categoryFactor As Double) As List(Of DevAVMonthlyIncome)
        Dim data = New List(Of DevAVMonthlyIncome)()
        Dim year = DateTime.Now.Year - 1
        Dim baseDate = New DateTime(year, 1, 1)
        Dim maxIncome = rnd.Next(60, 140)
        For i As Integer = 0 To 1000
            If (i Mod 100 = 0) Then
                maxIncome = Math.Max(40, rnd.Next(maxIncome - 20, maxIncome + 20))
            End If
            Dim month = endDate.AddDays(-i - 1)
            Dim income = rnd.Next(20, maxIncome) * companyFactor * categoryFactor
            data.Add(New DevAVMonthlyIncome With {
                     .Month = month,
                     .Income = income
            })
        Next i
        Return data
    End Function
End Class

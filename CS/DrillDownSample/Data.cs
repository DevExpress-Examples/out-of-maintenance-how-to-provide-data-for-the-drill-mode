using System;
using System.Collections.Generic;
using System.Linq;

namespace DrillDownSample {
    public interface INameable {
        string Name { get; }
    }

    public sealed class DevAVBranch: INameable {
        public string Name { get; set; }
        public List<DevAVProductCategory> Categories { get; set; }
        public double TotalIncome { get => Categories.Sum(c => c.TotalIncome); }
    }


    public sealed class DevAVProductCategory: INameable {
        public string Name { get; set; }
        public List<DevAVProduct> Products { get; set; }
        public double TotalIncome { get => Products.Sum(p => p.TotalIncome); }
    }


    public sealed class DevAVProduct: INameable {
        public string Name { get; set; }
        public List<DevAVMonthlyIncome> Sales { get; set; }
        public double TotalIncome { get => Sales.Sum(s => s.Income); }
    }


    public class DevAVMonthlyIncome {
        public DateTime Month { get; set; }
        public double Income { get; set; }
    }

    public class BranchDAO {
        readonly string[] companies = new string[] {
            "DevAV North",
            "DevAV South",
            "DevAV West",
            "DevAV East",
            "DevAV Central"
        };
        Dictionary<string, List<string>> categorizedProducts;
        readonly Random rnd = new Random(2019);
        DateTime endDate;

        Dictionary<string, List<string>> CategorizedProducts {
            get {
                if (this.categorizedProducts == null) {
                    this.categorizedProducts = new Dictionary<string, List<string>>();
                    this.categorizedProducts["Cell Phones"] = new List<string>() { "Smartphones", "Mobile Phones", "Smart Watches", "Sim Cards" };
                    this.categorizedProducts["Computers"] = new List<string>() { "PCs", "Laptops", "Tablets", "Printers" };
                    this.categorizedProducts["TV, Audio"] = new List<string>() { "TVs", "Home Audio", "Headphones", "DVD Players" };
                    this.categorizedProducts["Car Electronics"] = new List<string>() { "GPS Units", "Radars", "Car Alarms", "Car Accessories" };
                    this.categorizedProducts["Power Devices"] = new List<string>() { "Batteries", "Chargers", "Converters", "Testers", "AC/DC Adapters" };
                    this.categorizedProducts["Photo"] = new List<string>() { "Cameras", "Camcorders", "Binoculars", "Flashes", "Tripodes" };
                }
                return this.categorizedProducts;
            }
        }

        public BranchDAO() {
            DateTime now = DateTime.Now;
            this.endDate = new DateTime(now.Year, now.Month, 1);
        }

        public List<DevAVBranch> GetBranches() {
            List<DevAVBranch> data = new List<DevAVBranch>();
            foreach (string branchName in this.companies) {
                double companyFactor = rnd.NextDouble() * 0.6 + 1;
                List<DevAVProductCategory> categories = GenerateBranchSales(companyFactor);
                DevAVBranch branch = new DevAVBranch {
                    Name = branchName,
                    Categories = categories
                };
                data.Add(branch);
            }
            return data;
        }
        List<DevAVProductCategory> GenerateBranchSales(double companyFactor) {
            List<DevAVProductCategory> categories = new List<DevAVProductCategory>();
            foreach (var categoryProductsPair in CategorizedProducts) {
                double categoryFactor = rnd.NextDouble() * 0.6 + 1;
                List<DevAVProduct> products = GenerateCategoryProducts(categoryProductsPair, companyFactor, categoryFactor);
                DevAVProductCategory category = new DevAVProductCategory {
                    Name = categoryProductsPair.Key,
                    Products = products
                };
                categories.Add(category);
            }
            return categories;
        }
        List<DevAVProduct> GenerateCategoryProducts(KeyValuePair<string, List<string>> categoryProductsPair, double companyFactor, double categoryFactor) {
            List<DevAVProduct> products = new List<DevAVProduct>();
            foreach (string productName in categoryProductsPair.Value) {
                List<DevAVMonthlyIncome> sales = GenerateSalesForProduct(companyFactor, categoryFactor);
                DevAVProduct product = new DevAVProduct {
                    Name = productName,
                    Sales = sales
                };
                products.Add(product);
            }
            return products;
        }
        List<DevAVMonthlyIncome> GenerateSalesForProduct(double companyFactor, double categoryFactor) {
            List<DevAVMonthlyIncome> data = new List<DevAVMonthlyIncome>();
            int year = DateTime.Now.Year - 1;
            DateTime baseDate = new DateTime(year, 1, 1);
            int maxIncome = rnd.Next(60, 140);
            for (int i = 0; i < 1000; i++) {
                if (i % 100 == 0)
                    maxIncome = Math.Max(40, rnd.Next(maxIncome - 20, maxIncome + 20));
                DateTime month = endDate.AddDays(-i - 1);
                double income = rnd.Next(20, maxIncome) * companyFactor * categoryFactor;
                DevAVMonthlyIncome monthlyIncome = new DevAVMonthlyIncome {
                    Month = month,
                    Income = income
                };
                data.Add(monthlyIncome);
            }
            return data;
        }
    }
}
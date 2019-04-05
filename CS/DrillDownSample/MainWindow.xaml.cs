using DevExpress.Xpf.Charts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace DrillDownSample {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void XYDiagram2D_DrillDownStateChanged(object sender, DrillDownStateChangedEventArgs e) {
            diagram.Rotated = e.BreadcrumbItems.Last().IsHome;
        }
    }

    class DevAVSeriesChildrenSelector : IChildrenSelector {
        public IEnumerable SelectChildren(object item) {
            if (item is DevAVBranch)
                return new List<DevAVBranch> { (DevAVBranch)item };
            if (item is DevAVProductCategory)
                return ((DevAVProductCategory)item).Products;
            if (item is DevAVProduct)
                return new List<DevAVProduct> { (DevAVProduct)item };
            else
                return null;
        }
    }

    class DevAVSeriesTemplateSelector : DataTemplateSelector {
        public DataTemplate AllCategoriesTemplate { get; set; }
        public DataTemplate BranchCategoriesTemplate { get; set; }
        public DataTemplate CategoryProductsTemplate { get; set; }
        public DataTemplate ProductTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container) {
            Diagram diagram = (Diagram)container;
            if (item is DevAVBranch && diagram.BreadcrumbItems.Count == 1)
                return AllCategoriesTemplate;
            if (item is DevAVBranch)
                return BranchCategoriesTemplate;
            if (item is DevAVProduct && diagram.BreadcrumbItems[diagram.BreadcrumbItems.Count - 1].SourceObject is DevAVProduct)
                return ProductTemplate;
            if (item is DevAVProduct) 
                return CategoryProductsTemplate;
            return null;
        }
    }

    class DevAVBreadcrumbTextProvider : IBreadcrumbTextProvider {
        public string GetText(object seriesSourceObj, object pointSourceObj) {
            StringBuilder sb = new StringBuilder("(");
            if (seriesSourceObj != null) sb.Append(GetName(seriesSourceObj));
            if (seriesSourceObj != null && pointSourceObj != null) sb.Append(", ");
            if (pointSourceObj != null) sb.Append(GetName(pointSourceObj));
            sb.Append(")");
            return sb.ToString();
        }

        private string GetName(object obj) {
            if (obj is INameable nameable) return nameable.Name;
            return String.Empty;
        }
    }
}

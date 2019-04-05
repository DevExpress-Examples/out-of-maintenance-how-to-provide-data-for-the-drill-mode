using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DrillDownSample {
    class MainViewModel {
        public IReadOnlyList<DevAVBranch> Branches { get; } 

        public MainViewModel() {
            Branches = new BranchDAO().GetBranches();
        }
    }
}

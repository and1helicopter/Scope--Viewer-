using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication4
{
    using System.Collections.Generic;

    using OxyPlot;

    public class MainViewModel
    {
        public MainViewModel()
        {
            this.Title = "Example 2";
            this.Points1 = new List<DataPoint>
                              {
                                  new DataPoint(0, 4),
                                  new DataPoint(10, 13),
                                  new DataPoint(20, 15),
                                  new DataPoint(30, 16),
                                  new DataPoint(40, 12),
                                  new DataPoint(50, 12)
                              };
            this.Points2 = new List<DataPoint>
                              {
                                  new DataPoint(0, 14),
                                  new DataPoint(10, 3),
                                  new DataPoint(20, 5),
                                  new DataPoint(30, 6),
                                  new DataPoint(40, 2),
                                  new DataPoint(50, 2)
                              };
            this.Points3 = new List<DataPoint>
                               {

                               };
        }

        public string Title { get; private set; }

        public IList<DataPoint> Points1 { get; private set; }
        public IList<DataPoint> Points2 { get; private set; }
        public IList<DataPoint> Points3 { get; private set; }
    }
}

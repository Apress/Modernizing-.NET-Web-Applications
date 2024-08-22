using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModernizationDemo.App.Model
{
    public class PagerModel
    {
        public string ActionName { get; set; }

        public int PageIndex { get; set; }

        public int PagesCount { get; set; }
    }
}
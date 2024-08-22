using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModernizationDemo.AppNew.Model
{
    public class PagerModel
    {
        public string Url { get; set; }

        public int PageIndex { get; set; }

        public int PagesCount { get; set; }
    }
}
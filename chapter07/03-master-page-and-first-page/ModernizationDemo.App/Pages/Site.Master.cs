using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ModernizationDemo.App
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CurrencyList.SelectedValue = ((PageBase)Page).SelectedCurrency;
            }
        }

        protected void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ((PageBase)Page).SetCurrency(CurrencyList.SelectedValue);
        }
    }
}
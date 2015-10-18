/*  
 *  Copyright © 2012 Matthew David Elgert - mdelgert@yahoo.com
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation; either version 2.1 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA 
 * 
 *  Project URL: http://jquerymobile.codeplex.com/                           
 *  
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

namespace Web
{
    public partial class PayPal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //<a href="https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_xclick&business=seller_1244149184_biz@vessea.com&item_name=jQueryMobile Donation&currency_code=USD&amount=25.00&quantity=1&return=http://jquerymobile.vessea.com/Default.aspx&rm=2&cbt=Return to jQueryMobile&cancel_return=http://jquerymobile.vessea.com&notify_url=http://jquerymobile.vessea.com/IPN.aspx">Check Out</a>
            PayPalUrl.InnerHtml = BLL.IPN.CreatePaymentUrl("Donation", 25.00m, 1);
        }
    }
}
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
using System.Web;
using DAL;

namespace BLL.Survey
{

    public class Results
    {
        private EntitiesContext context;

        public Results(EntitiesContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets the value of the current Portal ID from the current Session.
        /// If the session value is null it returns 1.
        /// </summary>
        public int PortalID
        {
            get
            {
                object temp = HttpContext.Current.Session["PortalID"]; 
                return temp == null ? 1 : (int)temp;
            }
        }

        public Int32 Insert()
        {

            DAL.SurveyResult Record = new DAL.SurveyResult
            {
                CreateDate = DateTime.Now,
                PortalID = this.PortalID
            };

            this.context.AddToSurveyResults(Record);
            this.context.SaveChanges();
            return Record.ResultID;
        }

    }

}

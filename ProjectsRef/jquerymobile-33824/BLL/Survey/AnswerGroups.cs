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
using System.Collections;
using System.Linq;
using System.Web;
using DAL;

namespace BLL.Survey
{
    public static class AnswerGroups
    {
        /// <summary>
        /// Gets the value of the current Portal ID from the current Session.
        /// If the session value is null it returns 1.
        /// </summary>
        public static int PortalID
        {
            get
            {
                object temp = HttpContext.Current.Session["PortalID"];
                return temp == null ? 1 : (int)temp;
            }
        }
        
        /// <summary>
        /// Content RadTable functions
        /// </summary>

        public static IQueryable DataSource()
        {
            var query = from _SurveyAnswerGroups in Utils.DbContext.SurveyAnswerGroups
                        join _SurveyAnswersTypes in Utils.DbContext.SurveyAnswersTypes on _SurveyAnswerGroups.AnswerTypeID equals _SurveyAnswersTypes.SurveyAnswerTypeID
                        where _SurveyAnswerGroups.PortalID == AnswerGroups.PortalID
                        select new { 
                                     EntityKey = _SurveyAnswerGroups.AnswerGroupID, 
                                     _SurveyAnswerGroups.AnswerGroupID, 
                                     _SurveyAnswerGroups.AnswerTypeID, 
                                     _SurveyAnswerGroups.Name, 
                                     AnswerType = _SurveyAnswersTypes.Name };
            return query;
        }

        public static IQueryable DataSourceDD()
        {
            var query = from _SurveyAnswerGroups in Utils.DbContext.SurveyAnswerGroups
                        join _SurveyAnswersTypes in Utils.DbContext.SurveyAnswersTypes on _SurveyAnswerGroups.AnswerTypeID equals _SurveyAnswersTypes.SurveyAnswerTypeID
                        where _SurveyAnswerGroups.PortalID == AnswerGroups.PortalID
                        select new
                        {
                            EntityKey = _SurveyAnswerGroups.AnswerGroupID,
                            _SurveyAnswerGroups.AnswerGroupID,
                            _SurveyAnswerGroups.AnswerTypeID,
                            _SurveyAnswerGroups.Name,
                            AnswerType = _SurveyAnswersTypes.Name
                        };
            return query;
        }
        public static void Delete(int EntityKey)
        {
            DAL.SurveyAnswerGroup record = Utils.DbContext.SurveyAnswerGroups.Where(p => p.AnswerGroupID == EntityKey).FirstOrDefault();
            Utils.DbContext.DeleteObject(record);
            Utils.DbContext.SaveChanges();
        }

        public static void Update(int EntityKey, Hashtable values)
        {
            DAL.SurveyAnswerGroup record = Utils.DbContext.SurveyAnswerGroups.Where(p => p.AnswerGroupID == EntityKey).FirstOrDefault();
            record.Name = values["Name"].ToString();
            record.AnswerTypeID = int.Parse(values["AnswerTypeID"].ToString());
            Utils.DbContext.SaveChanges();
        }

        public static void Insert(Hashtable values)
        {

            DAL.SurveyAnswerGroup Record = new DAL.SurveyAnswerGroup
            {

                PortalID = AnswerGroups.PortalID,
                CreateDate = DateTime.Now,
                AnswerTypeID = int.Parse(values["AnswerTypeID"].ToString()),
                Name = values["Name"].ToString(),
                IsDeleted = false
            };

            Utils.DbContext.AddToSurveyAnswerGroups(Record);
            Utils.DbContext.SaveChanges();
        }

        /// <summary>
        /// Content RadTable functions
        /// </summary>

        public static Hashtable GetAnswerGroup(int EntityKey)
        {
            var values = new Hashtable();

            using (EntitiesContext context = new EntitiesContext())
            {
                var query = from q in Utils.DbContext.SurveyAnswerGroups
                            where q.AnswerGroupID == EntityKey
                            select new
                            {
                                q.AnswerGroupID,
                                q.PortalID,
                                q.CreateDate,
                                q.AnswerTypeID,
                                q.Name
                            };

                foreach (var i in query)
                {
                    values["AnswerGroupID"] = i.AnswerGroupID;
                    values["PortalID"] = i.PortalID;
                    values["InsertDT"] = i.CreateDate;
                    values["AnswerTypeID"] = i.AnswerTypeID;
                    values["Name"] = i.Name;
                }

            }
            return values;
        }
    }

}

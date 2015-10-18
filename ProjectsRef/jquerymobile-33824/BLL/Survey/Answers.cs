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

using System.Collections;
using System.Linq;
using System.Web;

namespace BLL.Survey
{
    public static class Answers
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
            int MaxDisplayOrder = GetMaxDisplayOrder();
            var query = from _SurveyAnswers in Utils.DbContext.SurveyAnswers
                        join _SurveyAnswerGroups in Utils.DbContext.SurveyAnswerGroups 
                              on _SurveyAnswers.AnswerGroupID equals _SurveyAnswerGroups.AnswerGroupID
                        where _SurveyAnswerGroups.PortalID == Answers.PortalID
                        orderby _SurveyAnswers.DisplayOrder
                        select new { 
                                   EntityKey = _SurveyAnswers.AnswerID, 
                                   _SurveyAnswers.Answer,
                                   AnswerGroup = _SurveyAnswerGroups.Name,
                                   MaxDisplayOrder
                                   };
            return query;
        }

        public static void Delete(int EntityKey)
        {
            DAL.SurveyAnswer record = Utils.DbContext.SurveyAnswers.Where(p => p.AnswerID == EntityKey).FirstOrDefault();
            record.IsDeleted = true;
            Utils.DbContext.SaveChanges();
        }

        public static void Update(int EntityKey, Hashtable values)
        {
            DAL.SurveyAnswer record = Utils.DbContext.SurveyAnswers.Where(p => p.AnswerID == EntityKey).FirstOrDefault();
            record.Answer = values["Answer"].ToString();
            record.AnswerGroupID = int.Parse(values["AnswerGroupID"].ToString());
            Utils.DbContext.SaveChanges();
        }

        public static void Insert(Hashtable values)
        {

            DAL.SurveyAnswer Record = new DAL.SurveyAnswer
            {
                Answer = values["Answer"].ToString(),
                AnswerGroupID = int.Parse(values["AnswerGroupID"].ToString()),
                DisplayOrder = GetMaxDisplayOrder() + 1,
                PortalID = Answers.PortalID,
                IsDeleted = false
            };

            Utils.DbContext.AddToSurveyAnswers(Record);
            Utils.DbContext.SaveChanges();
        }

        /// <summary>
        /// Content RadTable functions
        /// </summary>

        public static void MoveRecord(int EntityKey, bool Direction)
        {

            int MaxDisplayOrder = GetMaxDisplayOrder();
            int MinDisplayOrder = GetMinDisplayOrder();
            int SelectedDisplayOrder = GetSelectedDisplayOrder(EntityKey);
            var SelectedItem = Utils.DbContext.SurveyAnswers.Single(p => p.AnswerID == EntityKey);

            var query = from _SurveyAnswers in Utils.DbContext.SurveyAnswers
                        join _SurveyAnswerGroups in Utils.DbContext.SurveyAnswerGroups on _SurveyAnswers.AnswerGroupID equals _SurveyAnswerGroups.AnswerGroupID
                        where _SurveyAnswerGroups.PortalID == Answers.PortalID
                        select _SurveyAnswers;

            if (Direction == true & SelectedDisplayOrder != MinDisplayOrder) //Move record up
            {
                var MoveItem = Utils.DbContext.SurveyAnswers.Single(p => p.DisplayOrder == SelectedDisplayOrder - 1 & p.PortalID == Answers.PortalID);
                MoveItem.DisplayOrder = SelectedDisplayOrder;
                SelectedItem.DisplayOrder = SelectedDisplayOrder - 1;
                Utils.DbContext.SaveChanges();
            }

            if (Direction == true & SelectedDisplayOrder == MinDisplayOrder) //Move all records up 
            {
                foreach (var r in query)
                {
                    if (r.DisplayOrder != MinDisplayOrder)
                    {
                        r.DisplayOrder = r.DisplayOrder - 1;
                    }
                }
                SelectedItem.DisplayOrder = MaxDisplayOrder;
                Utils.DbContext.SaveChanges();
            }

            if (Direction == false & SelectedDisplayOrder != MaxDisplayOrder) //Move record down
            {
                var MoveItem = Utils.DbContext.SurveyAnswers.Single(p => p.DisplayOrder == SelectedDisplayOrder + 1 & p.PortalID == Answers.PortalID);
                MoveItem.DisplayOrder = SelectedDisplayOrder;
                SelectedItem.DisplayOrder = SelectedDisplayOrder + 1;
                Utils.DbContext.SaveChanges();
            }

            if (Direction == false & SelectedDisplayOrder == MaxDisplayOrder) //Move all records down
            {
                foreach (var r in query)
                {
                    if (r.DisplayOrder != MaxDisplayOrder)
                    {
                        r.DisplayOrder = r.DisplayOrder + 1;
                    }
                }
                SelectedItem.DisplayOrder = MinDisplayOrder;
                Utils.DbContext.SaveChanges();
            }

        }

        public static int GetMaxDisplayOrder()
        {
            int MaxDisplayOrder = 0;

            var query = (from _SurveyAnswers in Utils.DbContext.SurveyAnswers
                         join _SurveyAnswerGroups in Utils.DbContext.SurveyAnswerGroups on _SurveyAnswers.AnswerGroupID equals _SurveyAnswerGroups.AnswerGroupID
                         where _SurveyAnswerGroups.PortalID == Answers.PortalID
                         select _SurveyAnswers.DisplayOrder);

            if (query.Count() > 0)
            {
                MaxDisplayOrder = query.Max();
            }

            return (int)MaxDisplayOrder;
        }

        public static int GetMinDisplayOrder()
        {

            int MinDisplayOrder = 0;

            var query = (from _SurveyAnswers in Utils.DbContext.SurveyAnswers
                         join _SurveyAnswerGroups in Utils.DbContext.SurveyAnswerGroups on _SurveyAnswers.AnswerGroupID equals _SurveyAnswerGroups.AnswerGroupID
                         where _SurveyAnswerGroups.PortalID == Answers.PortalID
                         select _SurveyAnswers.DisplayOrder);

            if (query.Count() > 0)
            {
                MinDisplayOrder = query.Min();
            }

            return (int)MinDisplayOrder;

        }

        public static int GetSelectedDisplayOrder(int EntityKey)
        {
            System.Nullable<int> SelectedDisplayOrder = (from q in Utils.DbContext.SurveyAnswers
                                                           where q.AnswerID == EntityKey
                                                           select (int)q.DisplayOrder).Single();
            return (int)SelectedDisplayOrder;
        }

    }

}

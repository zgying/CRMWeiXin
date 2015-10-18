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
using DAL;

namespace BLL.Survey
{
    public static class Questions
    {
        /// <summary>
        /// Gets the value of the current Portal ID from the current Session.
        /// If the session value is null it returns 1.
        /// </summary>
        public static int PortalID
        {
            get { object temp = HttpContext.Current.Session["PortalID"]; 
            return temp == null ? 1 : (int)temp; }
        }

        /// <summary>
        /// Content RadTable functions
        /// </summary>

        public static IQueryable DataSource()
        {
            int MaxDisplayOrder = GetMaxDisplayOrder();
            var query = from _SurveyQuestions in Utils.DbContext.SurveyQuestions
                        join _Surveys in Utils.DbContext.Surveys on _SurveyQuestions.SurveyID equals _Surveys.SurveyID
                        join _SurveyAnswerGroups in Utils.DbContext.SurveyAnswerGroups on _SurveyQuestions.AnswerGroupID equals _SurveyAnswerGroups.AnswerGroupID
                        where _Surveys.PortalID == Questions.PortalID
                        orderby _SurveyQuestions.DisplayOrder
                        select new
                        {
                            EntityKey = _SurveyQuestions.QuestionID,
                            _SurveyQuestions.QuestionID,
                            _SurveyQuestions.SurveyID,
                            _SurveyQuestions.AnswerGroupID,
                            _SurveyQuestions.Question,
                            _SurveyQuestions.DisplayOrder,
                            _SurveyQuestions.IsDeleted,
                            AnswerGroupName = _SurveyAnswerGroups.Name,
                            SurveyName = _Surveys.Name,
                            MaxDisplayOrder
                        };
            return query;
        }

        public static void Delete(int EntityKey)
        {
            DAL.SurveyQuestion record = Utils.DbContext.SurveyQuestions.Where(p => p.QuestionID == EntityKey).FirstOrDefault();
            record.IsDeleted = true;
            Utils.DbContext.SaveChanges();
        }

        public static void Update(int EntityKey, Hashtable values)
        {
            DAL.SurveyQuestion record = Utils.DbContext.SurveyQuestions.Where(p => p.QuestionID == EntityKey).FirstOrDefault();
            record.Question = values["Question"].ToString();
            record.AnswerGroupID = int.Parse(values["AnswerGroupID"].ToString());
            record.SurveyID = int.Parse(values["SurveyID"].ToString());
            record.IsDeleted = bool.Parse(values["IsDeleted"].ToString());
            Utils.DbContext.SaveChanges();
        }

        public static void Insert(Hashtable values)
        {

            DAL.SurveyQuestion Record = new DAL.SurveyQuestion
            {
                Question = values["Question"].ToString(),
                AnswerGroupID = int.Parse(values["AnswerGroupID"].ToString()),
                SurveyID = int.Parse(values["SurveyID"].ToString()),
                DisplayOrder = GetMaxDisplayOrder() + 1,
                IsDeleted = bool.Parse(values["IsDeleted"].ToString()),
                PortalID = Questions.PortalID
            };

            Utils.DbContext.AddToSurveyQuestions(Record);
            Utils.DbContext.SaveChanges();
        }

        /// <summary>
        /// Content RadTable functions
        /// </summary>

        public static Hashtable GetSurveyQuestion(int EntityKey)
        {
            var values = new Hashtable();

            using (EntitiesContext context = new EntitiesContext())
            {
                var query = from q in Utils.DbContext.SurveyQuestions
                            where q.QuestionID == EntityKey
                            select new
                            {
                                q.QuestionID,
                                q.SurveyID,
                                q.AnswerGroupID,
                                q.Question,
                                q.IsDeleted
                            };

                foreach (var i in query)
                {
                    values["QuestionID"] = i.QuestionID;
                    values["SurveyID"] = i.SurveyID;
                    values["AnswerGroupID"] = i.AnswerGroupID;
                    values["Question"] = i.Question;
                    values["IsDeleted"] = i.IsDeleted;
                }
            }

            return values;
        }

        public static void MoveRecord(int EntityKey, bool Direction)
        {

            int MaxDisplayOrder = GetMaxDisplayOrder();
            int MinDisplayOrder = GetMinDisplayOrder();
            int SelectedDisplayOrder = GetSelectedDisplayOrder(EntityKey);
            var SelectedItem = Utils.DbContext.SurveyQuestions.Single(p => p.QuestionID == EntityKey);

            var query = from _SurveyQuestions in Utils.DbContext.SurveyQuestions
                        join _Surveys in Utils.DbContext.Surveys on _SurveyQuestions.SurveyID equals _Surveys.SurveyID
                        where _Surveys.PortalID == Questions.PortalID
                        select _SurveyQuestions;

            if (Direction == true & SelectedDisplayOrder != MinDisplayOrder) //Move record up
            {
                var MoveItem = Utils.DbContext.SurveyQuestions.Single(p => p.DisplayOrder == SelectedDisplayOrder - 1 & p.PortalID == Questions.PortalID);
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
                var MoveItem = Utils.DbContext.SurveyQuestions.Single(p => p.DisplayOrder == SelectedDisplayOrder + 1 & p.PortalID == Questions.PortalID);
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

            var query = (from _SurveyQuestions in Utils.DbContext.SurveyQuestions
                         join _Surveys in Utils.DbContext.Surveys on _SurveyQuestions.SurveyID equals _Surveys.SurveyID
                         where _Surveys.PortalID == Questions.PortalID
                         select _SurveyQuestions.DisplayOrder);

            if (query.Count() > 0)
            {
                MaxDisplayOrder = query.Max();
            }

            return (int)MaxDisplayOrder;
        }

        public static int GetMinDisplayOrder()
        {

            int MinDisplayOrder = 0;

            var query = (from _SurveyQuestions in Utils.DbContext.SurveyQuestions
                         join _Surveys in Utils.DbContext.Surveys on _SurveyQuestions.SurveyID equals _Surveys.SurveyID
                         where _Surveys.PortalID == Questions.PortalID
                         select _SurveyQuestions.DisplayOrder);

            if (query.Count() > 0)
            {
                MinDisplayOrder = query.Min();
            }

            return (int)MinDisplayOrder;

        }

        public static int GetSelectedDisplayOrder(int EntityKey)
        {
            System.Nullable<int> SelectedDisplayOrder = (from q in Utils.DbContext.SurveyQuestions
                                                           where q.QuestionID == EntityKey
                                                           select (int)q.DisplayOrder).Single();
            return (int)SelectedDisplayOrder;
        }

    }

}

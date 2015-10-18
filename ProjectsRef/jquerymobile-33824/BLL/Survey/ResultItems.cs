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
 * URLS:
 * http://www.codeproject.com/Tips/74914/LINQ-To-Entity-Outer-Join
 * 
 *  Project URL: http://jquerymobile.codeplex.com/                           
 *  
 */

using System.Linq;
using System.Text;
using System.Web;
using DAL;

namespace BLL.Survey
{
    public static class ResultItems
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
            var query = from _SurveyResultItems in Utils.DbContext.SurveyResultItems

                        join t1 in Utils.DbContext.SurveyAnswers on _SurveyResultItems.AnswerID equals t1.AnswerID into temp1
                        from _SurveyAnswers in temp1.DefaultIfEmpty()

                        join t2 in Utils.DbContext.SurveyQuestions on _SurveyResultItems.QuestionID equals t2.QuestionID into temp2
                        from _SurveyQuestions in temp2.DefaultIfEmpty()

                        join t3 in Utils.DbContext.SurveyAnswerGroups on _SurveyAnswers.AnswerGroupID equals t3.AnswerGroupID into temp3
                        from _SurveyAnswerGroups in temp3.DefaultIfEmpty()

                        join t4 in Utils.DbContext.Surveys on _SurveyQuestions.SurveyID equals t4.SurveyID into temp4
                        from _Surveys in temp4.DefaultIfEmpty()

                        where _Surveys.PortalID == ResultItems.PortalID
                        orderby _SurveyResultItems.ResultID

                        select new
                        {
                            ID = _SurveyResultItems.ResultID,
                            Survey = _Surveys.Name,
                            _SurveyQuestions.Question,
                            Answer = _SurveyAnswerGroups.AnswerTypeID == null ? _SurveyResultItems.AnswerText :
                                     _SurveyAnswerGroups.AnswerTypeID != null ? _SurveyAnswers.Answer : "-"
                        };

            return query;
        }

        /// <summary>
        /// Content RadTable functions
        /// </summary>

        public static string PostSurveyResults()
        {

            StringBuilder sb = new StringBuilder();

            var query1 = from _SurveyQuestions in Utils.DbContext.SurveyQuestions
                         join _SurveyAnswerGroups in Utils.DbContext.SurveyAnswerGroups on _SurveyQuestions.AnswerGroupID equals _SurveyAnswerGroups.AnswerGroupID
                         join _SurveyAnswersTypes in Utils.DbContext.SurveyAnswersTypes on _SurveyAnswerGroups.AnswerTypeID equals _SurveyAnswersTypes.SurveyAnswerTypeID
                         where _SurveyQuestions.IsDeleted != true & _SurveyQuestions.PortalID == ResultItems.PortalID
                         orderby _SurveyQuestions.DisplayOrder
                         select new
                         {
                             _SurveyQuestions.QuestionID,
                             _SurveyQuestions.AnswerGroupID,
                             _SurveyAnswersTypes.SurveyAnswerTypeID
                         };

            int ResultID = new Survey.Results(Utils.DbContext).Insert();

            foreach (var q1 in query1)
            {

                if (HttpContext.Current.Request.Form[q1.QuestionID.ToString()] != null)
                {
                    string ResultValue = HttpContext.Current.Request.Form[q1.QuestionID.ToString()];
                    string _AnswerText = null;
                    int _AnswerID = 0;

                    if (ResultValue != "")
                    {
                        if (q1.SurveyAnswerTypeID < 3)
                        {
                            _AnswerText = ResultValue;
                            _AnswerID = 0;
                        }

                        if (q1.SurveyAnswerTypeID > 3)
                        {
                            _AnswerID = int.Parse(ResultValue);
                        }

                        InsertResult(q1.QuestionID, _AnswerID, 0, _AnswerText, ResultID);

                    }

                }

            }

            sb.Append("<p>Thank you for taking the survey.</p>");

            return sb.ToString();

            //return BLL.Survey.Items.ReturnSurveyXML();

        }

        public static void InsertResult(int _QuestionID, int _AnswerID, int _UserID, string _AnswerText, int _ResultID)
        {

            using (EntitiesContext context = new EntitiesContext())
            {
                SurveyResultItem r = new SurveyResultItem
                {
                    ResultID = _ResultID,
                    QuestionID = _QuestionID,
                    AnswerID = _AnswerID,
                    UserID = _UserID,
                    AnswerText = _AnswerText,
                    PortalID = ResultItems.PortalID
                };
                context.SurveyResultItems.AddObject(r);
                context.SaveChanges();
            }


        }

    }

}

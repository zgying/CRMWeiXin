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
using System.Xml;

namespace BLL.Survey
{
    public static class Items
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
            var query = from q in Utils.DbContext.Surveys
                        where q.PortalID == Items.PortalID
                        select new { EntityKey = q.SurveyID, q.SurveyID, q.Name };
            return query;
        }

        public static void Delete(int EntityKey)
        {
            DAL.Survey record = Utils.DbContext.Surveys.Where(p => p.SurveyID == EntityKey).FirstOrDefault();
            Utils.DbContext.DeleteObject(record);
            Utils.DbContext.SaveChanges();
        }

        public static void Update(int EntityKey, Hashtable values)
        {
            DAL.Survey record = Utils.DbContext.Surveys.Where(p => p.SurveyID == EntityKey).FirstOrDefault();
            record.Name = values["Name"].ToString();
            Utils.DbContext.SaveChanges();
        }

        public static void Insert(Hashtable values)
        {

            DAL.Survey Record = new DAL.Survey
            {
                Name = values["Name"].ToString(),
                PortalID = Items.PortalID,
                CreateDate = DateTime.Now,
                IsDeleted = false
            };

            Utils.DbContext.AddToSurveys(Record);
            Utils.DbContext.SaveChanges();
        }

        /// <summary>
        /// Content RadTable functions
        /// </summary>

        public static string ReturnSurveyXML()
        {

            if (HttpContext.Current.Request.Form["SurveyID"] != null)
            {

                int ID = 1;
                string _SurveyName = "";
                int SurveyID = int.Parse(HttpContext.Current.Request.Form["SurveyID"]);

                XmlDocument doc = new XmlDocument();
                XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
                doc.AppendChild(dec);

                XmlElement Root = doc.CreateElement("Root");
                doc.AppendChild(Root);

                var query1 = from _SurveyQuestions in Utils.DbContext.SurveyQuestions
                             join _SurveyAnswerGroups in Utils.DbContext.SurveyAnswerGroups on _SurveyQuestions.AnswerGroupID equals _SurveyAnswerGroups.AnswerGroupID
                             join _SurveyAnswersTypes in Utils.DbContext.SurveyAnswersTypes on _SurveyAnswerGroups.AnswerTypeID equals _SurveyAnswersTypes.SurveyAnswerTypeID
                             join _Surveys in Utils.DbContext.Surveys on _SurveyQuestions.SurveyID equals _Surveys.SurveyID
                             where _SurveyQuestions.IsDeleted != true & _SurveyAnswerGroups.PortalID == Items.PortalID & _Surveys.SurveyID == SurveyID
                             orderby _SurveyQuestions.DisplayOrder
                             select new
                             {
                                 _SurveyQuestions.QuestionID,
                                 _SurveyQuestions.Question,
                                 _SurveyQuestions.AnswerGroupID,
                                 _SurveyAnswersTypes.SurveyAnswerTypeID,
                                 SurveyName = _Surveys.Name
                             };

                foreach (var q1 in query1)
                {

                    _SurveyName = q1.SurveyName;

                    XmlElement Rows = doc.CreateElement("Rows");
                    Root.AppendChild(Rows);

                    XmlElement QuestionID = doc.CreateElement("QuestionID");
                    QuestionID.InnerText = q1.QuestionID.ToString();
                    Rows.AppendChild(QuestionID);

                    XmlElement Question = doc.CreateElement("Question");
                    Question.InnerText = q1.Question;
                    Rows.AppendChild(Question);

                    XmlElement AnswerType = doc.CreateElement("AnswerType");
                    AnswerType.InnerText = q1.SurveyAnswerTypeID.ToString();
                    Rows.AppendChild(AnswerType);

                    XmlElement SurveyAnswers = doc.CreateElement("SurveyAnswers");
                    Rows.AppendChild(SurveyAnswers);

                    var query2 = from _SurveyAnswers in Utils.DbContext.SurveyAnswers
                                 where _SurveyAnswers.AnswerGroupID == q1.AnswerGroupID
                                 orderby _SurveyAnswers.DisplayOrder
                                 select new
                                 {
                                     _SurveyAnswers.AnswerID,
                                     _SurveyAnswers.AnswerGroupID,
                                     _SurveyAnswers.Answer
                                 };

                    foreach (var q2 in query2)
                    {
                        XmlElement Record = doc.CreateElement("Record");
                        SurveyAnswers.AppendChild(Record);

                        XmlElement RowID = doc.CreateElement("RowID");
                        RowID.InnerText = ID.ToString();
                        Record.AppendChild(RowID);

                        XmlElement ParrentQuestionID = doc.CreateElement("ParrentQuestionID");
                        ParrentQuestionID.InnerText = q1.QuestionID.ToString();
                        Record.AppendChild(ParrentQuestionID);

                        XmlElement AnswerID = doc.CreateElement("AnswerID");
                        AnswerID.InnerText = q2.AnswerID.ToString();
                        Record.AppendChild(AnswerID);

                        XmlElement Answer = doc.CreateElement("Answer");
                        Answer.InnerText = q2.Answer;
                        Record.AppendChild(Answer);

                        ID = ID + 1;
                    }

                }

                XmlElement Survey = doc.CreateElement("Survey");
                Root.AppendChild(Survey);

                XmlElement Name = doc.CreateElement("Name");
                Name.InnerText = _SurveyName;
                Survey.AppendChild(Name);

                return doc.TransformXSLT("UserSurvey.xslt");
            }
            else
            {
                return "No Survey Setup";
            }

        }

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using DAL;
using BLL;

namespace Web.App_Master
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        private EntitiesContext context;
        private Portals portals;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.context = new EntitiesContext();
            this.portals = new Portals(context);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
             
                //Public Site
                RadMenuItem item2 = new RadMenuItem("Public Site");
                item2.NavigateUrl = "~/Default.aspx";
                RadMenu1.Items.Add(item2);

                //Admin
                RadMenuItem item3 = new RadMenuItem("Admin");
                item3.NavigateUrl = "~/Admin/Default.aspx";
                RadMenu1.Items.Add(item3);

                //Portals
                if (Session["IsSuperUser"] != null)
                {
                    if ((bool)Session["IsSuperUser"] == true)
                    {
                        RadMenuItem item4 = new RadMenuItem("Portals");
                        RadMenu1.Items.Add(item4);

                        RadMenuItem childItem1 = new RadMenuItem("Edit Portals");
                        childItem1.NavigateUrl = "~/Admin/AddEditPortals.aspx";
                        item4.Items.Add(childItem1);

                        RadMenuItem childItem2 = new RadMenuItem("Edit Portal Alias");
                        childItem2.NavigateUrl = "~/Admin/AddEditPortalAlias.aspx";
                        item4.Items.Add(childItem2);

                    }
                }

                //Store
                RadMenuItem item5 = new RadMenuItem("Store");
                RadMenu1.Items.Add(item5);

                RadMenuItem childItem3 = new RadMenuItem("Categories");
                childItem3.NavigateUrl = "~/Admin/Store/AddEditCategories.aspx";
                item5.Items.Add(childItem3);

                RadMenuItem childItem4 = new RadMenuItem("Products");
                childItem4.NavigateUrl = "~/Admin/Store/AddEditProduct.aspx";
                item5.Items.Add(childItem4);

                RadMenuItem childItem5 = new RadMenuItem("Variant Groups");
                childItem5.NavigateUrl = "";
                item5.Items.Add(childItem5);

                RadMenuItem childItem6 = new RadMenuItem("Variants");
                childItem6.NavigateUrl = "";
                item5.Items.Add(childItem6);

                //Survey
                RadMenuItem item6 = new RadMenuItem("Survey");
                RadMenu1.Items.Add(item6);

                RadMenuItem childItem7 = new RadMenuItem("Surveys");
                childItem7.NavigateUrl = "~/Admin/Survey/AddEditSurvey.aspx";
                item6.Items.Add(childItem7);

                RadMenuItem childItem8 = new RadMenuItem("Answer Groups");
                childItem8.NavigateUrl = "~/Admin/Survey/AddEditAnswerGroups.aspx";
                item6.Items.Add(childItem8);

                RadMenuItem childItem9 = new RadMenuItem("Answers");
                childItem9.NavigateUrl = "~/Admin/Survey/AddEditAnswers.aspx";
                item6.Items.Add(childItem9);

                RadMenuItem childItem10 = new RadMenuItem("Questions");
                childItem10.NavigateUrl = "~/Admin/Survey/AddEditQuestions.aspx";
                item6.Items.Add(childItem10);

                RadMenuItem childItem11 = new RadMenuItem("Results");
                childItem11.NavigateUrl = "~/Admin/Survey/Results.aspx";
                item6.Items.Add(childItem11);

                //Users
                RadMenuItem item7 = new RadMenuItem("Users");
                item7.NavigateUrl = "~/Admin/AddEditUsers.aspx";
                RadMenu1.Items.Add(item7);

                //Roles
                RadMenuItem item8= new RadMenuItem("Roles");
                item8.NavigateUrl = "~/Admin/AddEditRoles.aspx";
                RadMenu1.Items.Add(item8);

                //Pages
                RadMenuItem item9 = new RadMenuItem("Pages");
                item9.NavigateUrl = "~/Admin/AddEditPages.aspx";
                RadMenu1.Items.Add(item9);

                //Logs
                if (Session["IsSuperUser"] != null)
                {
                    if ((bool)Session["IsSuperUser"] == true)
                    {
                        RadMenuItem item10 = new RadMenuItem("Logs");
                        RadMenu1.Items.Add(item10);

                        RadMenuItem childItem12 = new RadMenuItem("Site Log");
                        childItem12.NavigateUrl = "~/Admin/SiteLog.aspx";
                        item10.Items.Add(childItem12);

                        RadMenuItem childItem13 = new RadMenuItem("Elmah");
                        childItem13.NavigateUrl = "/Elmah.axd";
                        item10.Items.Add(childItem13);

                        //Pages
                        RadMenuItem item11 = new RadMenuItem("File Manager");
                        item11.NavigateUrl = "~/Admin/FileManager.aspx";
                        RadMenu1.Items.Add(item11);

                    }
                
                }

                //Must be last to highlight an item
                RadMenuItem items = RadMenu1.FindItemByUrl(Request.Url.PathAndQuery);
                if (items != null)
                {
                    items.HighlightPath();
                }

                ddPortal.DataSource = this.portals.DataSource();
                ddPortal.DataTextField = "Name";
                ddPortal.DataValueField = "PortalID";
                ddPortal.DataBind();
                ddPortal.SelectedValue = Session["PortalID"].ToString();

                if (Session["IsSuperUser"] != null)
                {
                    if ((bool)Session["IsSuperUser"] == true)
                    {
                        ChangePortal1.Visible = true;
                        ChangePortal2.Visible = true;
                    }
                }

            }  

        }

        protected void ddPortal_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["PortalID"] = Int32.Parse(ddPortal.SelectedValue);
            Response.Redirect(Request.RawUrl);
        }

    }

}
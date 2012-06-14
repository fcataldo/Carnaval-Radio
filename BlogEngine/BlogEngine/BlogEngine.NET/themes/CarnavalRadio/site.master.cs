﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using BlogEngine.Core;
using ExtensionMethods;

public partial class StandardSite : System.Web.UI.MasterPage
{
  protected void Page_Load(object sender, EventArgs e)
  {
      if (Security.IsAuthenticated)
      {
          aUser.InnerText = "Welcome " + Page.User.Identity.Name + "!";
          aLogin.InnerText = Resources.labels.logoff;
          aLogin.HRef = Utils.RelativeWebRoot + "Account/login.aspx?logoff";
      }
      else
      {
          aLogin.HRef = Utils.RelativeWebRoot + "Account/login.aspx";
          aLogin.InnerText = Resources.labels.login;
      }
      RegisterStyleSheetInclude(string.Format("{0}{1}", Utils.AbsoluteWebRoot,
                                              "themes/CarnavalRadio/styles/superfish.css"));
      RegisterClientScriptInclude(string.Format("{0}{1}", Utils.AbsoluteWebRoot, "themes/CarnavalRadio/js/superfish.js"));
      litMenu.Text = buildMenu("");
      litSponsorImages.Text = getSponsorImages();
  }

    private string getSponsorImages()
    {
        StringBuilder sb = new StringBuilder();
        //<a href="http://www.test1.nl"><img src="http://cloud.github.com/downloads/malsup/cycle/beach1.jpg" width="220" height="86" /></a>
        foreach (CRSponsor crs in CRSponsor.GetListOnlyActives().Where(i => i.WidgetSwitch))
        {
            sb.AppendFormat("<a href=\"{0}\" title=\"{1}\"><img src=\"{2}\" alt=\"{1}\" title=\"{1}\" width=\"222\" height=\"86\" /></a>", crs.Url.ToUrlString(), crs.Name, crs.LogoURL);
        }
        return sb.ToString();
    }

    private string buildMenu(string currentPage)
    {
        StringBuilder menu = new StringBuilder();

        foreach (var parentPage in BlogEngine.Core.Page.Pages.Where(p => !p.HasParentPage))
        {
            menu.AppendFormat("<li class=\"page_item\"><a href=\"{0}\" title=\"{1}\">{1}</a>", parentPage.RelativeLink, parentPage.Title);

            if (parentPage.HasChildPages)
            {
                menu.Append("<ul class=\"sub-menu\">");
                foreach (
                    var childPage in
                        BlogEngine.Core.Page.Pages.Where(p => p.Parent == parentPage.Id))
                {
                    menu.AppendFormat(
                        "<li class=\"page_item\"><a href=\"{0}\" title=\"{1}\">{1}</a></li>",
                        childPage.RelativeLink, childPage.Title);
                }
                menu.AppendFormat("</ul>");
            }

            menu.Append("</li>");
        }

        return menu.ToString();
    }

    /// <summary>
    /// Registers the client script include.
    /// </summary>
    /// <param name="src">The file name.</param>
    private void RegisterClientScriptInclude(string src)
    {
        var si = new System.Web.UI.HtmlControls.HtmlGenericControl();
        si.TagName = "script";
        si.Attributes.Add("type", "text/javascript");
        si.Attributes.Add("src", src);
        this.Page.Header.Controls.Add(si);
        this.Page.Header.Controls.Add(new LiteralControl("\n"));
    }

    /// <summary>
    /// Registers the client script include.
    /// </summary>
    /// <param name="src">The file name.</param>
    private void RegisterStyleSheetInclude(string src)
    {
        var si = new System.Web.UI.HtmlControls.HtmlGenericControl();
        si.TagName = "link";
        si.Attributes.Add("type", "text/css");
        si.Attributes.Add("rel", "stylesheet");
        si.Attributes.Add("media", "screen");
        si.Attributes.Add("href", src);
        this.Page.Header.Controls.Add(si);
        this.Page.Header.Controls.Add(new LiteralControl("\n"));
    }

}

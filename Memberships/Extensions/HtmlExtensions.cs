using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Memberships.Extensions
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString GlyphLink(this HtmlHelper helper, string controller, string action, string text, string glyphicon, string cssClasses="", string id = "")
        {
            var glyph = string.Format($"<span class='glyphicon glyphicon-{glyphicon}'></span>");

            var achor = new TagBuilder("a");
            achor.MergeAttribute("href", string.Format($"/{controller}/{action}/"));
            achor.InnerHtml = string.Format($"{glyph} {text}");
            achor.AddCssClass(cssClasses);
            achor.GenerateId(id);

            return MvcHtmlString.Create(achor.ToString(TagRenderMode.Normal));
        }
    }
}
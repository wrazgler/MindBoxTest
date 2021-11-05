using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

using MindBoxTest2.ViewModels;

namespace MindBoxTest2.TagHelpers
{
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;
        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public PageViewModel PageModel { get; set; }
        public string PageAction { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "div";

            var tag = new TagBuilder("ul");
            tag.AddCssClass("pagination");
            tag.AddCssClass("justify-content-center");

            if (PageModel.HasPreviousPage)
            {
                var prevItem = CreateTag(PageModel.PageNumber - 1, urlHelper, true);
                prevItem.MergeAttribute("style", "margin-right: 10px;");
                tag.InnerHtml.AppendHtml(prevItem);
                prevItem = CreateTag(PageModel.PageNumber - 1, urlHelper, false);
                prevItem.MergeAttribute("style", "margin-right: 10px;");
                tag.InnerHtml.AppendHtml(prevItem);
            }

            var currentItem = CreateTag(PageModel.PageNumber, urlHelper, false);
            tag.InnerHtml.AppendHtml(currentItem);


            if (PageModel.HasNextPage)
            {
                var nextItem = CreateTag(PageModel.PageNumber + 1, urlHelper, false);
                nextItem.MergeAttribute("style", "margin-left: 10px;");
                tag.InnerHtml.AppendHtml(nextItem);
                nextItem = CreateTag(PageModel.PageNumber + 1, urlHelper, true);
                nextItem.MergeAttribute("style", "margin-left: 10px;");
                tag.InnerHtml.AppendHtml(nextItem);
            }
            output.Content.AppendHtml(tag);
        }

        TagBuilder CreateTag(int pageNumber, IUrlHelper urlHelper, bool corner)
        {
            var item = new TagBuilder("li");
            var link = new TagBuilder("a");
            if (pageNumber == this.PageModel.PageNumber)
            {
                link.AddCssClass("active");
            }
            else
            {
                PageUrlValues["page"] = pageNumber;
                link.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
            }

            link.AddCssClass("page-item");
            link.AddCssClass("btn");
            link.AddCssClass("btn-outline-dark");
            link.AddCssClass("text-center");

            if (!corner)
            {
                link.InnerHtml.Append(pageNumber.ToString());
                link.MergeAttribute("style", "width: 40px");
            }
            if (pageNumber == (this.PageModel.PageNumber + 1) && corner)
            {
                link.InnerHtml.Append("Вперёд");
                link.MergeAttribute("style", "width: 125px");
            }
            if (pageNumber == (this.PageModel.PageNumber - 1) && corner)
            {
                link.InnerHtml.Append("Назад");
                link.MergeAttribute("style", "width: 125px");
            }

            item.InnerHtml.AppendHtml(link);
            return item;
        }
    }
}

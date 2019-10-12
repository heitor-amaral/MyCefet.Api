using HtmlAgilityPack;
using System.Collections.Generic;

namespace MyCefet.Api.Extensions
{
    public static class HtmlNodeExtensions
    {
        public static string SelectNodeByXPathAndIndex(this HtmlNode htmlNode, string xpath, int index = -1)
        {
            if(index>=0)
            {
                return htmlNode.SelectNodes(xpath)[index].InnerText;
            }
                return htmlNode.SelectSingleNode(xpath).InnerText;
        }
    }
}

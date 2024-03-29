﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SimpleSiteMap
{
    public class SitemapService
    {
        private const string SitemapsNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";
#if NETSTANDARD1_2
        private static readonly string ByteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble(), 0, Encoding.UTF8.GetPreamble().Length);
#else
        private static readonly string ByteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
#endif
        /// <summary>
        /// Creates a sitemap for the given collection.
        /// </summary>
        /// <remarks>The resultant sitemap will create links to a route that will generate urlset's.</remarks>
        /// <param name="sitemapNodes">the collection of sitemap nodes to render.</param>
        /// <returns></returns>
        public string ConvertToXmlSitemap(ICollection<SitemapNode> sitemapNodes)
        {
            if (sitemapNodes == null)
            {
                throw new ArgumentNullException(nameof(sitemapNodes));
            }

            var xElement = CreateXmlSitemapIndex(sitemapNodes);

            return xElement != null
                ? ConvertXElementToString(xElement)
                : null;
        }

        /// <summary>
        /// Creates a urlset for the given collection.
        /// </summary>
        /// <remarks>The resultant urlset will create links to the actual resource to be searched.</remarks>
        /// <param name="sitemapNodes">the collection of sitemap nodes to render.</param>
        /// <returns></returns>
        public string ConvertToXmlUrlset(ICollection<SitemapNode> sitemapNodes)
        {
            if (sitemapNodes == null)
            {
                throw new ArgumentNullException(nameof(sitemapNodes));
            }

            var xElement = CreateXmlUrlSet(sitemapNodes);

            return xElement != null
                ? ConvertXElementToString(xElement)
                : null;
        }

        private static XElement CreateXmlSitemapIndex(ICollection<SitemapNode> sitemapNodes)
        {
            if (sitemapNodes == null)
            {
                throw new ArgumentNullException();
            }

            XNamespace xmlns = SitemapsNamespace;

            var root = new XElement(xmlns + "urlset");

            if (sitemapNodes.Any())
            {
                // Display each 'page' link.
                var page = 0;
                foreach (var sitemapNode in sitemapNodes)
                {
                    page++;
                    
                    var loc = GenerateLocXElement(sitemapNode, page, sitemapNode.AppendPageQueryParam);
                    
                    var lastMod = new XElement(xmlns + "lastmod",
                        sitemapNode.LastModified.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture));

                    root.Add(new XElement(xmlns + "url", loc, lastMod));
                }
            }

            return root;
            
            XElement GenerateLocXElement(SitemapNode sitemapNode, int page, bool appendPageQueryParam)
            {
                var uriString = sitemapNode.Url.ToString();
                
               

                return new XElement(xmlns + "loc", uriString);
            }
        }

        private static XElement CreateXmlUrlSet(IEnumerable<SitemapNode> sitemapNodes)
        {
            if (sitemapNodes == null)
            {
                throw new ArgumentNullException(nameof(sitemapNodes));
            }

            XNamespace xmlns = SitemapsNamespace;

            var root = new XElement(xmlns + "urlset");

            foreach (var node in sitemapNodes)
            {
                var urlElement = new XElement(xmlns + "url");

                var loc = new XElement(xmlns + "loc", Uri.EscapeUriString(node.Url.AbsoluteUri));
                urlElement.Add(loc);

                var lastMod = new XElement(xmlns + "lastmod",
                    node.LastModified.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture));
                urlElement.Add(lastMod);

                if (node.Frequency.HasValue)
                {
                    var changeFrequency = new XElement(xmlns + "changefreq", node.Frequency.Value.ToString().ToLowerInvariant());
                    urlElement.Add(changeFrequency);
                }

                if (node.Priority.HasValue)
                {
                    var priority = new XElement(xmlns + "priority", node.Priority.Value.ToString(CultureInfo.InvariantCulture).ToLowerInvariant());
                    urlElement.Add(priority);
                }

                root.Add(urlElement);
            }

            return root;
        }

        private static string ConvertXElementToString(XElement xElement)
        {
            if (xElement == null)
            {
                throw new ArgumentNullException(nameof(xElement));
            }

            // Convert the xml to a string.
            string xmlResult;
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
                {
                    xElement.Save(writer);
                }

                var array = memoryStream.ToArray();
#if NETSTANDARD1_2
                xmlResult = Encoding.UTF8.GetString(array, 0, array.Length);
#else
                xmlResult = Encoding.UTF8.GetString(array);
#endif
            }

            if (!string.IsNullOrWhiteSpace(xmlResult) &&
                xmlResult.StartsWith(ByteOrderMarkUtf8))
            {
                xmlResult = xmlResult.Remove(0, ByteOrderMarkUtf8.Length);
            }

            return xmlResult;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions; 
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Threading;
using System.IO;
using System.Linq;

namespace TikiCrawl
{
    class Program
    {
        static void Main()
        {
            IWebDriver browser = new ChromeDriver();

            List<string> listProductLink = new List<string>(); 
            List<string> listUrl = new List<string>();
            string url;
            //navigate to website
            for (int i = 1; i < 10; i++)
            {
                url = "https://tiki.vn/search?q=giay&page=" + i;
                browser.Navigate().GoToUrl(url);
                Thread.Sleep(2000);
                var products = browser.FindElements(By.CssSelector(".product-item"));
                foreach (var product in products)
                {
                    string outerHtml = product.GetAttribute("outerHTML");
                    string productLink = Regex.Match(outerHtml, "href=\"(.*?)\"").Groups[1].Value;
                    productLink = "https://tiki.vn" + productLink;
                    listProductLink.Add(productLink);
                }

            }
            TextWriter writer = new System.IO.StreamWriter("C:\\Users\\Admin\\Desktop\\tikipub.csv", false, System.Text.Encoding.UTF8);
            writer.WriteLine("{0}", "publisherName");
            for (int i = 1; i <= 5; i++)
            {
                //Go to product link
                browser.Navigate().GoToUrl("https://www.vinabook.com/index.php?dispatch=publishers.view_all&page=" + i);
                Thread.Sleep(1500);
                var publishers = browser.FindElements(By.CssSelector(".list-authors li a"));
                foreach ( var publisher in publishers)
                {
                    writer.WriteLine("{0}", publisher.Text);
                }
                //extract product information by css selector
                string producttitle = browser.FindElement(By.CssSelector(".mainbox-title")).Text;
                Thread.Sleep(1000);
                //extract product price
                var productCurrentPrice = browser.FindElement(By.CssSelector(".product-price__current-price")).Text;
                productCurrentPrice = productCurrentPrice.Replace("\r", "").Replace("\n", "").Replace(",", "").Replace("₫", "").Replace(".", "");
                //extract product images
                string productimage = browser.FindElement(By.CssSelector(".cm-image-wrap")).GetAttribute("outerhtml");
                productimage = Regex.Match(productimage, "src=\"(.*?)\"").Groups[1].Value;

                string productImage = browser.FindElement(By.CssSelector(".product-image-feature")).GetAttribute("src");
                //extract colors
                List<string> colorlist = new List<string>();
                var colorSpans = browser.FindElements(By.CssSelector(".option-label"));
                {
                    foreach (var colorSpan in colorSpans)
                    {
                        string color = colorSpan.Text;
                        colorlist.Add(color);
                    }
                }
                //extract sizes
                List<string> sizelist = new List<string>();
                var sizespans = browser.FindElements(By.CssSelector(".styles__optionbutton-sc-3p38uy-0"));
                {
                    foreach (var sizespan in sizespans)
                    {
                        string size = sizespan.Text;
                        sizelist.Add(size);
                    }
                }
                char amoc = '@';
                string stringlistsizes = string.Join(amoc, sizelist);
                string stringlistcolors = string.Join(amoc, colorlist);
                //extract product description
                string productDescription = browser.FindElement(By.CssSelector(".togglecontent__view-sc-1hm81e2-0")).Text;
                ((IJavaScriptExecutor)browser).ExecuteScript("window.scrollto(0, document.body.scrollheight - 2000)");
                Thread.Sleep(400);
                string productdescription = browser.FindElement(By.CssSelector(".content")).Text;
                productdescription = productdescription.Replace("\r", " ").Replace("\n", " ").Replace(",", " ").Replace("   ", "  ").Replace("  ", " ");
                string produc = producttitle + " chắc chắn là món phụ kiện thời trang nữ mà các cô gái đều cần đến nếu muốn có style điệu đà và nổi bật giữa đám bạn thân. chỉ cần chải tóc gọn gàng và kết hợp thêm băng đô cài tóc hcm thìbạn đã ghi điểm hơn nhiều so với thường lệ.";
                writer.WriteLine("{0}", productimage);
            }
            writer.Close();
            //writer1.Close();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

// Creating some automation for accessibility based on the .gov checklist https://accessibility.18f.gov/checklist/

namespace AAT
{
    [TestClass]
    public class UnitTest1
    {
        private static readonly IWebDriver Driver = new ChromeDriver();


        [TestMethod]
        public void TestPageAccessibility()
        {
            // Navigate to page to test
            Driver.Navigate().GoToUrl("https://google.co.uk/");

            // run some basic accessibility checks
            var inputsWithoutExplicityLabels = GetInputsWithoutExplicitLabels();

            var numberOfImagesWithoutAltTags = GetNumberOfImagesWithoutAltAttributes();

            var brokenLinks = GetNumberOfBrokenLinks();

            // Do some assertions or write out to a report....
            // do something!!

            // Close web driver, test finished
            Driver.Quit();
        }

        // All form inputs have explicit labels
        public static List<string> GetInputsWithoutExplicitLabels()
        {
            var pageInputIds =
                Driver.FindElements(By.XPath("//input[@type='text' or @type='password']"))
                    .Select(x => x.GetAttribute("id"));
            var pageLabelFors = Driver.FindElements(By.TagName("label")).Select(x => x.GetAttribute("for"));

            return pageInputIds.Where(p => pageLabelFors.All(p2 => p2 != p)).ToList();
        }

        // All images have alt attributes
        public static int GetNumberOfImagesWithoutAltAttributes()
        {
            var pageImages = Driver.FindElements(By.TagName("img"));

            return pageImages.Count(p => string.IsNullOrWhiteSpace(p.GetAttribute("alt")));
        }

        public static int GetNumberOfBrokenLinks()
        {
            var links = Driver.FindElements(By.TagName("a"));

            // URL is either not configured for anchor tag or it is empty
            return links.Where(x => string.IsNullOrWhiteSpace(x.GetAttribute("href"))).Count();

            // could also try and do request to see if response code is ok as long as url starts with/contains same domain url?! 
            // - wouldnt want to keep hitting external sites with requests!
        }
    }
}
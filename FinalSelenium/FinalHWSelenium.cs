using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using System.Windows.Forms;
using FormKeys = System.Windows.Forms.Keys;
using SeleniumKeys = OpenQA.Selenium.Keys;

namespace FinalSelenium
{
    [TestFixture]
    public class FinalHWSelenium
    {
        IWebDriver Driver;
        string password = "Pass123";
        string email = "ristovski.goce@gmail.com";
        string pickUpAdresa = "Skopje";
        string deliveryAdresa = "Dresden";
        string requestTitle = "Brandy Cases";
        string pickUpDate = ("10" + "." + "04" + "." + "2022");
        string weight = "1,300";
        string volume = "2";
        string opisIPopis = "Destilerija Ristovski, premium quality, koj probal, znae :)";
        string totalTruck = "Total Truck";
        WebDriverWait wait;
        Random random;

        [SetUp]

        public void SetUp()
        {
            Driver = new ChromeDriver();
            random = new Random();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(50);
            Driver.Manage().Window.Maximize();
            Driver.Navigate().GoToUrl("http://18.156.17.83:9095/");
            wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            Language();
            LogIn();
            
        }

        [Test]
        public void FinallAssignment()
        {
            ACreateRequest();
            BCheckMyRequest();
            CHOME();
            DCancelation();
            
            
        }
        

        [TearDown]

        public void PisiBrisi()
        {

            
            Driver.Quit();
            Driver.Dispose();
        }

        public void Language()
        {
            IWebElement jazik = Driver.FindElement(By.CssSelector("span[translate = 'global.menu.language']"));
            jazik.Click();
            List<IWebElement> englishTab = Driver.FindElements(By.CssSelector("a[ng-click='languageVm.changeLanguage(language);vm.collapseNavbar();']")).ToList();
            IWebElement english = englishTab.First();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a[ng-click='languageVm.changeLanguage(language);vm.collapseNavbar();']")));
            //Thread.Sleep(1000);
            english.Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a[href='/privacy-policy']")));
            //Thread.Sleep(2000);
        }

        public void LogIn()
        {
            IWebElement Najava = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("span[translate=	'global.menu.account.login']")));
            Najava.Click();

            IWebElement KorisnickoIme = Driver.FindElement(By.Id("username"));
            KorisnickoIme.Clear();
            KorisnickoIme.SendKeys(email);

            IWebElement Lozinka = Driver.FindElement(By.Id("password"));
            Lozinka.Clear();
            Lozinka.SendKeys(password);

            IWebElement NajaviSe = Driver.FindElement(By.CssSelector("button[translate='login.form.button']"));
            NajaviSe.Click();

            IWebElement Najavuvanje = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("nav[role='navigation']")));

        }

        public void ACreateRequest()
        {
            IWebElement createRequest = Driver.FindElement(By.CssSelector("span[translate='provider.createRequest'"));
            createRequest.Click();

            IWebElement title = Driver.FindElement(By.Name("title"));
            title.Clear();
            title.SendKeys(requestTitle);

            IWebElement category = Driver.FindElement(By.Name("categoryType"));
            List<IWebElement> categoryTypes = category.FindElements(By.TagName("option")).ToList();
            IWebElement totalTruck = category.FindElement(By.CssSelector("option[translate='cargoApp.CategoryType.TOTAL_TRUCK']"));
            totalTruck.Click();
            string truck = totalTruck.Text;



            IWebElement pickUp = Driver.FindElement(By.CssSelector("input[ng-value='vm.address.formattedAddress']"));
            pickUp.SendKeys(pickUpAdresa);
            Thread.Sleep(4000);
            pickUp.SendKeys(OpenQA.Selenium.Keys.ArrowDown + SeleniumKeys.Enter);
            List<IWebElement> deliveryField = Driver.FindElements(By.CssSelector("input[ng-value='vm.address.formattedAddress']")).ToList();
            IWebElement delivery = deliveryField.Last();
            delivery.SendKeys(deliveryAdresa);
            Thread.Sleep(2000);
            delivery.SendKeys(SeleniumKeys.ArrowDown + SeleniumKeys.Enter);

            IWebElement setPickUpDate = Driver.FindElement(By.Id("setPickUpDate"));
            setPickUpDate.Click();
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("input[ng-model='vm.request.earliestPickUpDate']")));
            IWebElement enterPickUpDate = Driver.FindElement(By.CssSelector("input[ng-model='vm.request.earliestPickUpDate']"));
            enterPickUpDate.SendKeys(pickUpDate + SeleniumKeys.Tab + pickUpDate);

            IWebElement setDeliveryDate = Driver.FindElement(By.Id("setDeliveryUpDate"));
            setDeliveryDate.Click();
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("input[ng-model='vm.request.earliestDeliveryDate']")));
            IWebElement enterDeliveryDate = Driver.FindElement(By.CssSelector("input[ng-model='vm.request.earliestDeliveryDate']"));
            enterDeliveryDate.SendKeys(pickUpDate + SeleniumKeys.Tab + pickUpDate);

            IWebElement shipmentWeight = Driver.FindElement(By.Name("shipmentWeight"));
            shipmentWeight.SendKeys(weight);

            IWebElement shipmentVolume = Driver.FindElement(By.Name("shipmentVolume"));
            shipmentVolume.SendKeys(volume);

            IWebElement description = Driver.FindElement(By.CssSelector("textarea[ng-model='vm.request.description']"));
            description.SendKeys(opisIPopis);

            List<IWebElement> trucks = Driver.FindElements(By.CssSelector("input[checklist-model='vm.request.truckTypes']")).ToList();


            int randomBroj = random.Next(2, 10);
            IWebElement[] randomCategory = trucks.OrderBy(c => random.Next()).ToArray();

            for (int i = 0; i < randomBroj; i++)
            {

                //Thread.Sleep(4000);
                randomCategory[i].Click();

            }

            List<IWebElement> pay = Driver.FindElements(By.CssSelector("input[checklist-model='vm.paymentTypes']")).Take(3).ToList();
            //List<IWebElement> pay = Driver.FindElements(By.CssSelector("div[class='col-sm-8 col-sm-offset-2']")).ToList();
            //List<IWebElement> checkPay = pay.Where(e => e.Text.Contains("I can pay in")).ToList();
            //IWebElement payFinal = checkPay.FindAll(By.CssSelector(".ng-hide"));
            foreach (IWebElement checkBox in pay)
            {
                checkBox.Click();
                //checkBox.FindElement(By.CssSelector("input[ng_model='cheked']")).Click();
                //Thread.Sleep(1000);
            }

            IWebElement submitRequest = Driver.FindElement(By.CssSelector("input[value='Submit Request']"));
            submitRequest.Click();
        }

        public void BCheckMyRequest()
        {
            
            IWebElement checkName = Driver.FindElement(By.CssSelector("a[ui-sref='client-request-details({id: request.id})']"));
            //Assert.AreEqual(requestTitle, checkName.Text);
            IWebElement pickUp = Driver.FindElement(By.ClassName("table-body__row")).FindElement(By.ClassName("column2")).FindElements(By.TagName("span")).Last();
            //Assert.AreEqual(pickUpAdresa, pickUp.Text);
            IWebElement delivery = Driver.FindElement(By.ClassName("table-body__row")).FindElement(By.ClassName("column3")).FindElements(By.TagName("span")).Last();
            //Assert.AreEqual(deliveryAdresa, delivery.Text);
            IWebElement shipmentWeight = Driver.FindElement(By.ClassName("table-body__row")).FindElement(By.ClassName("column4")).FindElement(By.CssSelector("span[ng-show='request.shipmentWeight']")).FindElements(By.TagName("span")).Last();
            //Assert.AreEqual(weight + ".00 kg", shipmentWeight.Text);
            IWebElement shipmentVolumen = Driver.FindElement(By.ClassName("table-body__row")).FindElement(By.ClassName("column4")).FindElement(By.CssSelector("span[ng-show='request.shipmentVolume']")).FindElements(By.TagName("span")).Last();
            //Assert.AreEqual(Convert.ToInt32(volume) + ".00 m3", shipmentVolumen.Text);
            IWebElement truckType = Driver.FindElement(By.ClassName("table-body__row")).FindElement(By.ClassName("column5")).FindElement(By.TagName("span"));
            //Assert.AreEqual(totalTruck, truckType.Text);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(requestTitle, checkName.Text);
                Assert.AreEqual(pickUpAdresa, pickUp.Text);
                Assert.AreEqual(deliveryAdresa, delivery.Text);
                Assert.AreEqual(weight + ".00 kg", shipmentWeight.Text);
                Assert.AreEqual(Convert.ToInt32(volume) + ".00 m3", shipmentVolumen.Text);
                Assert.AreEqual(totalTruck, truckType.Text);

            });
        }
        
        public void CHOME()
        {
            IWebElement home = Driver.FindElement(By.CssSelector("a[href='/client/home']"));
            home.Click();
            
            BCheckMyRequest();
        }

        public void DCancelation()
        {
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("span[translate='provider.myRequests']")));
            IWebElement myRequest = Driver.FindElement(By.CssSelector("span[translate='provider.myRequests']"));
            myRequest.Click();
            IWebElement cancelElement = Driver.FindElement(By.CssSelector("a[ui-sref='client-request-details({id: request.id})']"));
            cancelElement.Click();
            IWebElement cancelRequest = Driver.FindElement(By.CssSelector("button[ng-click='vm.cancelRequest()']"));
            cancelRequest.Click();
        }
    }
 }

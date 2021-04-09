using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cmc.Core.ComponentModel;
using Cmc.Core.Configuration;
using Cmc.Core.PaymentProvider.Interfaces;
using Cmc.Core.PaymentProvider.Messages;

namespace TouchnetMVCDemo.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        public ActionResult Index()
        {
           
           

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            var payload = new GetOrRedirectToProviderUrlRequest()
            {
                PaymentRequest = new PaymentRequest()
                {
                    GenerateTokenOnly = true,
                    IsAutomatedClearingHouse = false,
                    IsRedirectEnabled = true,
                    //ReturnUrl = GetBaseUrl() + "Core/api/Payment/PaymentReceipt/return",
                    //PostBackUrl = GetBaseUrl() + "Core/api/Payment/PaymentReceipt/return",
                    ReturnUrl = GetBaseUrl() + "Core/api/Payment/PaymentReceiptWebHooks/return",
                    PostBackUrl = GetBaseUrl() + "Core/api/Payment/PaymentReceiptWebHooks/return",
                    UserDefinedField1 = Guid.NewGuid().ToString(), // used for reference
                    UserDefinedField2 = "Touchnet",
                    UserDefinedField4 = "1",
                    UserDefinedField7="digest",
                    CorrelationId = Guid.NewGuid().ToString(),
                    TransactionType = "S",
                    //ProviderInfo = new PaymentProviderInfo
                    //{
                    //    MerchantCode = "191",
                    //    Partner = "Touchnet",
                    //    UserName = "hob$on$",
                    //    Password = "Ya4raZak",
                    //    PaymentGatewayUrl = "https://test.secure.touchnet.net:8703/C30002test_tlink/services/TPGSecureLink",
                    //    HostedPageUrl = "https://test.secure.touchnet.net:8443/C30002test_upay/web/index.jsp",
                    //}
                    ProviderInfo = new PaymentProviderInfo
                    {
                        MerchantCode = "CAMPUS",
                        Partner = "CAMPUS",
                        UserName = "hob$on$",
                        Password = "Ya4raZak",
                        PaymentGatewayUrl = "https://test.secure.touchnet.net:8703/C30002test_tlink/services/TPGSecureLink",
                        HostedPageUrl = "https://train.cashnet.com/campuscheckouttest",
                    }
                    //ProviderInfo = new PaymentProviderInfo
                    //{
                    //    MerchantCode = "cmchetpc",
                    //    Partner = "PayPal",
                    //    UserName = "cmchetpc",
                    //    Password = "test@123",
                    //    PaymentGatewayUrl = "https://pilot-payflowpro.paypal.com/",
                    //    HostedPageUrl = "https://payflowlink.paypal.com?MODE=TEST",
                    //}
                    //ProviderInfo = new PaymentProviderInfo
                    //{
                    //    UserName = "5M2cKbUm9qzw",  //APILoginID
                    //    Password = "2ac45qVnQT9uX76T",  //APITransactionKey
                    //    PaymentGatewayUrl = "https://apitest.authorize.net/xml/v1/request.api",
                    //    HostedPageUrl = "https://test.authorize.net/payment/payment",
                    //},
                    //UserDefinedField3 = GetBaseUrl() + "Core/api/Payment/PaymentReceipt/return",

                },
                PayeePaymentDetails = new PayeePaymentDetails()
                {
                    Amount = 150,
                    LastName = "Moana",
                    FirstName = "Kaipo",
                    Email = "ayyappa@cmc.com",
                    Address = "777 Yamato Rd",
                    City = "Boca Raton",
                    State = "Alabama",
                    Zip = "33413",
                    Country = "United States",
                    Comment = "Comment from constructed URL - Doe, William (ID:943060 StuNum:DO09173336)"
                }
            };
            var config = ServiceLocator.Default.GetInstance<IConfigurationManager>();
            //config.AppSettings.Set("PaymentProvider", "Cashnet");
            config.AppSettings.Set("PaymentProvider", "Paypal");
            //config.AppSettings.Set("EncryptMethod", "md5");
            var paymentServices = ServiceLocator.Default.GetAllInstances<IPaymentAdaptor>();
            var paymentAdaptor = paymentServices.FirstOrDefault(svc => svc.GetType().Name.IndexOf("Cashnet", StringComparison.OrdinalIgnoreCase) >= 0);

            var paymentResponse = paymentAdaptor?.GetOrRedirectToProviderUrl(payload);
            if (paymentResponse?.PaymentException != null)
            {
                //var configuration = InvokePluginUsingFetchExpression(model, new PluginInputData { PaymentProvider = model.PaymentProvider }, "PaymentGatewayConfigurationService").FirstOrDefault();
            }

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private string GetBaseUrl()
        {
            return string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
        }
    }
}
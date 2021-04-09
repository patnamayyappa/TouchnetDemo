
using System;
using System.Linq;

using System.Web.Mvc;
using Cmc.Core.ComponentModel;
using Cmc.Core.PaymentProvider.Interfaces;
using Cmc.Core.PaymentProvider.Messages;

namespace TouchnetDemo
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var payload = new GetOrRedirectToProviderUrlRequest()
            {
                PaymentRequest = new PaymentRequest()
                {
                    GenerateTokenOnly = true,
                    IsAutomatedClearingHouse = false,
                    IsRedirectEnabled = true,
                    ReturnUrl = GetBaseUrl() + "Core/api/Payment/PaymentReceipt/return",
                    PostBackUrl = GetBaseUrl() + "Core/api/Payment/PaymentReceipt/return",
                    UserDefinedField1 = Guid.NewGuid().ToString(), // used for reference
                    UserDefinedField2 = "User Defined Field 2" + DateTime.Now.Ticks,
                    CorrelationId = Guid.NewGuid().ToString(),
                    TransactionType = "A",
                    ProviderInfo = new PaymentProviderInfo
                    {
                        MerchantCode = "191",
                        Partner = "Touchnet",
                        UserName = "hob$on$",
                        Password = "Ya4raZak",
                        PaymentGatewayUrl = "https://test.secure.touchnet.net:8703/C30002test_tlink/services/TPGSecureLink",
                        HostedPageUrl = "https://test.secure.touchnet.net:8443/C30002test_upay/web/index.jsp",
                    }
                },
                PayeePaymentDetails = new PayeePaymentDetails()
                {
                    Amount = 2,
                    //LastName = "Moana",
                    //FirstName = "Kaipo",
                    //Email = "Kaipo.Moana@cmc.local",
                    //Address = "777 Yamato Rd",
                    //City = "Boca Raton",
                    //State = "FL",
                    //Zip = "33413",
                    //Country = "USA",
                    //Comment = "Comment from constructed URL - Doe, William (ID:943060 StuNum:DO09173336)"
                }
            };

            var paymentServices = ServiceLocator.Default.GetAllInstances<IPaymentAdaptor>();
            var paymentAdaptor = paymentServices.FirstOrDefault(svc => svc.GetType().Name.IndexOf("Touchnet", StringComparison.OrdinalIgnoreCase) >= 0);

            var paymentResponse = paymentAdaptor?.GetOrRedirectToProviderUrl(payload);
            if (paymentResponse?.PaymentException != null)
            {
                //var configuration = InvokePluginUsingFetchExpression(model, new PluginInputData { PaymentProvider = model.PaymentProvider }, "PaymentGatewayConfigurationService").FirstOrDefault();
            }

        }

        private string GetBaseUrl()
        {
            return string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Controller.Url.Content("~"));
        }
    }
}
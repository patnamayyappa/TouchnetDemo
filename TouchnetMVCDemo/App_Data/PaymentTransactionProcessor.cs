using System;
using System.Linq;
using System.Web;
using Cmc.Core.ComponentModel;
using Cmc.Core.Diagnostics;
using Cmc.Core.PaymentProvider.Interfaces;
using Cmc.Core.PaymentProvider.Messages;

namespace TouchnetMVCDemo.App_Data
{
    [Injectable(InstanceScope.Factory)]
    internal class PaymentTransactionProcessor : IPaymentTransactionCallback
    {
        private readonly ILogger _logger;

        public PaymentTransactionProcessor(ILogger logger)
        {
            _logger = logger;
        }


        public void PostTransactionData(PaymentTransactionDetails transactionDetails)
        {
            using (new LogScope(_logger))
            {
                _logger.Debug("Posting Payflow transaction data to formsbuilder. Transaction Id = {0}, LastName={1}",
                    transactionDetails.TransactionResponse.TransactionCode,
                    transactionDetails.TransactionResponse.Lastname);

                string localRedirectUrl = null;

                if (transactionDetails.Operation == "cancel")
                {
                    localRedirectUrl = transactionDetails.TransactionResponse.UserDefinedField1;

                }
                else if (transactionDetails.Operation == "error")

                    localRedirectUrl = transactionDetails.TransactionResponse.UserDefinedField1;

                else if (transactionDetails.Operation == "return")
                {
                    string paymentException = transactionDetails.TransactionResponse.PaymentException == null ? "" : transactionDetails.TransactionResponse.PaymentException.ExceptionMessage;
                    long exceptionCode = 0;
                    if (paymentException != "")
                    {
                        if (transactionDetails.TransactionResponse.PaymentException != null)
                            exceptionCode = transactionDetails.TransactionResponse.PaymentException.ExceptionCode;
                    }

                    var paymentServices = ServiceLocator.Default.GetAllInstances<IPaymentAdaptor>();
                    var paymentAdaptor = paymentServices.FirstOrDefault(svc => svc.GetType().Name.IndexOf("Touchnet", StringComparison.OrdinalIgnoreCase) >= 0);

                    var paymentResponse = paymentAdaptor?.VerifyPayment(new VerifyPaymentRequest()
                    {
                        CorrelationId = transactionDetails.TransactionResponse.TransactionCode,
                        TransactionId = transactionDetails.TransactionResponse.SecureToken,
                        PaymentProviderInfo =  new PaymentProviderInfo()
                        {
                            MerchantCode = "191",
                            Partner = "Touchnet",
                            UserName = "hob$on$",
                            Password = "Ya4raZak",
                            PaymentGatewayUrl = "https://test.secure.touchnet.net:8703/C30002test_tlink/services/TPGSecureLink",
                            HostedPageUrl = "https://test.secure.touchnet.net:8443/C30002test_upay/web/index.jsp",
                        }

                    });
                    if (paymentResponse?.PaymentException != null)
                    {
                        //var configuration = InvokePluginUsingFetchExpression(model, new PluginInputData { PaymentProvider = model.PaymentProvider }, "PaymentGatewayConfigurationService").FirstOrDefault();
                    }

                    localRedirectUrl =
                        string.Format("~/PaymentReceipt?firstname={0}&lastname={1}&pnref={2}&exp={3}&lastfour={4}&paymentExceptionCode={5}&paymentException={6}&authcode={7}&pageTitle=Hosted Page Response",
                        transactionDetails.TransactionResponse.Firstname, transactionDetails.TransactionResponse.Lastname,
                        transactionDetails.TransactionResponse.TransactionCode, transactionDetails.TransactionResponse.ExpiryMonthYear,
                        transactionDetails.TransactionResponse.LastFour,
                        exceptionCode, paymentException,

                            transactionDetails.TransactionResponse.Authcode
                        );
                }
                else
                {
                    _logger.Error("Could not process payment for: " + transactionDetails.TransactionResponse.Firstname + " " +
                        transactionDetails.TransactionResponse.Lastname);
                }

                _logger.Debug("Redirecting to {0}", localRedirectUrl);

                HttpContext.Current.Response.Redirect(localRedirectUrl);
            }
        }
    }
}


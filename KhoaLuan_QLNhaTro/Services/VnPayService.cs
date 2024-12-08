﻿using VnPayIntegration.Models;
public class VnPayService : IVnPayService
{
    private readonly IConfiguration _configuration;

    public VnPayService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreatePaymentUrl(PaymentInformationModel model, HttpContext context)
    {
        var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
        var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
        var tick = DateTime.Now.Ticks.ToString();
        var pay = new VnPayLibrary();
        var urlCallBack = _configuration["Vnpay:PaymentBackReturnUrl"];

        pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
        pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
        pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
        pay.AddRequestData("vnp_Amount", ((int)model.Total * 100).ToString()); // Đảm bảo Amount nhân với 100
        pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
        pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
        pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
        pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
        pay.AddRequestData("vnp_OrderInfo", $"{model.RoomName} {model.Total} {model.BillId}");
        pay.AddRequestData("vnp_OrderType","Payment");
        pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
        pay.AddRequestData("vnp_TxnRef", tick);


        var paymentUrl = pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

        return paymentUrl;
    }
    //public string CreatePaymentUrl(PaymentInformationModel model, HttpContext context)
    //{
    //    var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
    //    var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
    //    var tick = DateTime.Now.Ticks.ToString();
    //    var pay = new VnPayLibrary();
    //    var urlCallBack = _configuration["Vnpay:PaymentBackReturnUrl"];

    //    pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
    //    pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
    //    pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
    //    pay.AddRequestData("vnp_Amount", ((int)Math.Round(model.Total * 100)).ToString()); // Đảm bảo Amount nhân với 100 và làm tròn
    //    pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
    //    pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
    //    pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
    //    pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
    //    pay.AddRequestData("vnp_OrderInfo", Uri.EscapeDataString($"{model.RoomName} {model.Total} {model.BillId}"));
    //    pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
    //    pay.AddRequestData("vnp_TxnRef", tick);

    //    var paymentUrl = pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

    //    return paymentUrl;
    //}

    public PaymentResponseModel PaymentExecute(IQueryCollection collections)
    {
        var pay = new VnPayLibrary();
        var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);

        return response;
    }
}
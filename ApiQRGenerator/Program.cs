using QRCoder;

namespace ApiQRGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.MapGet("/", (string qrContent) =>
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);
                BitmapByteQRCode bitmapByteQRCodeq = new BitmapByteQRCode(qrCodeData);
                var bitMap = bitmapByteQRCodeq.GetGraphic(20);

                using var ms = new MemoryStream();
                ms.Write(bitMap);
                byte[] data = ms.ToArray();
                var result = Convert.ToBase64String(data);
                return result;
            });
            app.Run();
        }
    }
}
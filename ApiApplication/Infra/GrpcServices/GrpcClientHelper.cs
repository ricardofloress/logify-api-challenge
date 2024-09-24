using System.Net.Http;
using Grpc.Net.Client;
using ProtoDefinitions;

namespace ApiApplication.GrpcServices.GrpcServices
{
    public class GrpcClientHelper
    {
        public static MoviesApi.MoviesApiClient GetClient()
        {
            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            var channel =
                GrpcChannel.ForAddress("https://api:443", new GrpcChannelOptions()
                {
                    HttpHandler = httpHandler
                });

            return new MoviesApi.MoviesApiClient(channel);
        }
    }
}
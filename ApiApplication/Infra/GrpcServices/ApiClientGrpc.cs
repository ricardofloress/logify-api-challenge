using System.Threading.Tasks;
using ApiApplication.Infra.GrpcServices.Interfaces;
using Grpc.Core;
using ProtoDefinitions;

namespace ApiApplication.GrpcServices.GrpcServices
{
    public class ApiClientGrpc : IApiClientGrpc
    {
        private readonly Metadata _headerApiKey = new Metadata()
            { { "X-Apikey", "68e5fbda-9ec9-4858-97b2-4a8349764c63" } };

        public async Task<showListResponse> GetAll()
        {
            var client = GrpcClientHelper.GetClient();

            var all = await client.GetAllAsync(new Empty(), _headerApiKey);

            all.Data.TryUnpack<showListResponse>(out var data);

            return data;
        }

        public async Task<showResponse> GetById(string id)
        {
            var client = GrpcClientHelper.GetClient();

            var show = client.GetById(new IdRequest() { Id = id }, _headerApiKey);

            show.Data.TryUnpack<showResponse>(out var data);

            return data;
        }
    }
}
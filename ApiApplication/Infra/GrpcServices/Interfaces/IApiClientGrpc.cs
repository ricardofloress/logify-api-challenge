using System.Threading.Tasks;
using ProtoDefinitions;

namespace ApiApplication.Infra.GrpcServices.Interfaces
{
    public interface IApiClientGrpc
    {
        Task<showListResponse> GetAll();
        Task<showResponse> GetById(string id);
    }
}
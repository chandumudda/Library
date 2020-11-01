using LibraryCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryCore.HttpClients
{
    public interface ILibraryClient
    {
        Task<HttpResponse> ExternalGetApiResultAsync(string webServiceUrl, Dictionary<string, string> headers = null);
        Task<HttpResponse> ExternalPatchApiResultAsync(string webServiceUrl, string jsonInput, Dictionary<string, string> headers = null);
        Task<HttpResponse> ExternalPostApiResultAsync(string webServiceUrl, string jsonInput, Dictionary<string, string> headers = null);
        Task<HttpResponse> ExternalDeleteApiResultAsync(string webServiceUrl, Dictionary<string, string> headers = null);
        public string GenerateAccessToken(User user);
    }
}

using System.Net.Http.Json;

namespace OuterSource
{
    public class Gravitel
    {
        private HttpClient _client;
        private string Final_Gravitel_URL = "";
        private const string Gravitel_Portal = "https://crm.aicall.ru";
        
        //Your Gravitel URL
        private string Client_Gravitel_URI = "/v1/{domain}"; 
        //Your Gravitel Token
        private string GravitelToken = "token";

        public Gravitel(string? portalUrl = null, string? token = null)
        {
            if (!string.IsNullOrWhiteSpace(portalUrl))
                Client_Gravitel_URI = portalUrl;
            if (!string.IsNullOrWhiteSpace(token))
                GravitelToken = token;
            Final_Gravitel_URL = Gravitel_Portal + Client_Gravitel_URI;

            HttpClient client = new();
            client.BaseAddress = new Uri(Final_Gravitel_URL);
            client.DefaultRequestHeaders.Add("X-API-KEY", GravitelToken);

            _client = client;
        }

        //Connection method for configure OAuth and access 
        
        public async Task<string?> SendCommand(HttpMethod methodType, string Command, string Params = "", string Body = "")
        {
            var final_request_url = Final_Gravitel_URL + '/' + Command;
            if (!string.IsNullOrEmpty(Params))
                final_request_url += "?" + Params;

            var request = new HttpRequestMessage(methodType, Final_Gravitel_URL + '/' + Command);
            
            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();

            return content;
        }

        public async Task<T?> SendCommand<T>(HttpMethod methodType, string Command, string Params = "", string Body = "")
        {
            var final_request_url = Final_Gravitel_URL + '/' + Command;
            if (!string.IsNullOrEmpty(Params))
                final_request_url += "?" + Params;

            var request = new HttpRequestMessage(methodType, Final_Gravitel_URL + '/' + Command);

            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadFromJsonAsync<T>();

            return content;
        }
    }
}

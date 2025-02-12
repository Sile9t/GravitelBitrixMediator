namespace Services.Contracts
{
    public interface IGravitel
    {
        Task<string?> SendCommand(HttpMethod methodType, string Command, string Params = "", string Body = "");
        Task<T?> SendCommand<T>(HttpMethod methodType, string Command, string Params = "", string Body = "");
    }
}

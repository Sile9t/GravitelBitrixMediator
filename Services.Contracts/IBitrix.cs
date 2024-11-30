namespace Services.Contracts
{
    public interface IBitrix
    {
        string SendCommand(string Command, string Params = "", string Body = "");
    }
}

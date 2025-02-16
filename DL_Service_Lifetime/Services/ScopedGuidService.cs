namespace DL_Service_Lifetime.Services;

public class ScopedGuidService: IScopedGuidService
{
    private readonly Guid ID;

    public ScopedGuidService()
    {
        ID = Guid.NewGuid();
    }
    public string GetGuid()
    {
        return ID.ToString();
    }
}
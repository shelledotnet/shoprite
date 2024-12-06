namespace ShopRite.Application.Exceptions
{
    //this uses primary constructor
    public class ItemNotFoundException(string message) : Exception(message)
    {
    }
}

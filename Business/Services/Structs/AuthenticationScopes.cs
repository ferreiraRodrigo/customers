namespace Customers.Business.Services.Structs
{
    public struct AuthenticationScopes
    {
        public const string ReadCustomer = "read:customer";
        public const string WriteCustomer = "write:customer";
        public const string ReadWishList = "read:wishlist";
        public const string WriteWishList = "write:wishlist";
    }
}

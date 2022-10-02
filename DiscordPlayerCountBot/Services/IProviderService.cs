namespace PlayerCountBot.Services
{
    public interface IProviderService<T> where T : IViewModelConverter
    {
        public Task<T?> GetInformation(string search, string? token = null);
    }
}

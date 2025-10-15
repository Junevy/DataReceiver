namespace DataReceiver.Services.Navigation
{
    internal interface INavigation
    {
        public void NavigateTo<T>() where T : class;
    }
}
